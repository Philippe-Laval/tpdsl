/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestReg
{
    /// <summary>
    /// Subclass the AssemblerParser to actually implement the necessary
    /// symbol table management and code generation functions.
    /// </summary>
    public class BytecodeAssembler : AssemblerParser
    {
        public const int INITIAL_CODE_SIZE = 1024;

        protected Dictionary<string, int> instructionOpcodeMapping = new Dictionary<string, int>();
        /// <summary>
        /// label scope
        /// </summary>
        protected Dictionary<string, LabelSymbol> labels = new Dictionary<string, LabelSymbol>();
        
        /// <summary>
        /// All float and string literals have unique int index in constant pool. 
        /// We put FunctionSymbols in here too.
        /// </summary>
        protected List<object> constPool = new List<object>();
        protected int ip = 0; // Instruction address pointer; used to fill code
        protected byte[] code = new byte[INITIAL_CODE_SIZE]; // code memory
        protected int dataSize; // set via .globals
        protected FunctionSymbol mainFunction = null!;

        /// <summary>
        /// Create an assembler attached to a lexer and define the instruction set.
        /// </summary>
        /// <param name="lexer"></param>
        /// <param name="instructions"></param>
        public BytecodeAssembler(ITokenStream lexer, Instruction[] instructions)
            : base(lexer)
        {
            for (int i = 1; i < instructions.Length; i++)
            {
                if (instructions[i] != null)
                {
                    instructionOpcodeMapping.Add(instructions[i].Name.ToLowerInvariant(), i);
                }
            }
        }

        public byte[] GetMachineCode() { return code; }

        public int GetCodeMemorySize() { return ip; }

        public int GetDataSize() { return dataSize; }

        /// <summary>
        /// Return the address associated with label "main:" if defined
        /// </summary>
        /// <returns></returns>
        public FunctionSymbol GetMainFunction() { return mainFunction; }

        /// <summary>
        /// Generate code for an instruction with no operands
        /// </summary>
        /// <param name="instrToken"></param>
        protected override void Gen(IToken instrToken)
        {
            //System.out.println("Gen "+instrToken);
            String instrName = instrToken.Text;
            int? opcodeI = instructionOpcodeMapping[instrName];
            if (opcodeI == null)
            {
                Console.WriteLine("line " + instrToken.Line +
                                   ": Unknown instruction: " + instrName);
                return;
            }
            int opcode = opcodeI.Value;
            EnsureCapacity(ip + 1);
            code[ip++] = (byte)(opcode & 0xFF);
        }

        /// <summary>
        /// Generate code for an instruction with one operand
        /// </summary>
        /// <param name="instrToken"></param>
        /// <param name="operandToken"></param>
        protected override void Gen(IToken instrToken, IToken operandToken)
        {
            Gen(instrToken);
            GenOperand(operandToken);
        }

        protected override void Gen(IToken instrToken, IToken oToken1, IToken oToken2)
        {
            Gen(instrToken, oToken1);
            GenOperand(oToken2);
        }

        protected override void Gen(IToken instrToken, IToken oToken1, IToken oToken2, IToken oToken3)
        {
            Gen(instrToken, oToken1, oToken2);
            GenOperand(oToken3);
        }

        protected void GenOperand(IToken operandToken)
        {
            string text = operandToken.Text;
            int v = 0;
            switch (operandToken.Type)
            { // switch on token type
                case INT: v = int.Parse(text); break;
                case CHAR: v = text[0]; break;
                case FLOAT: v = GetConstantPoolIndex(float.Parse(text)); break;
                case STRING: v = GetConstantPoolIndex(text); break;
                case ID: v = GetLabelAddress(text); break;
                case FUNC: v = GetFunctionIndex(text); break;
                case REG: v = GetRegisterNumber(operandToken); break;
            }
            EnsureCapacity(ip + 4);  // expand code array if necessary
            WriteInt(code, ip, v); // write operand to code byte array
            ip += 4;               // we've written four bytes
        }

        protected int GetConstantPoolIndex(object o)
        {
            if (constPool.Contains(o)) return constPool.IndexOf(o);
            constPool.Add(o);
            return constPool.Count - 1;
        }

        public object[] GetConstantPool() 
        {
            return constPool.ToArray(); 
        }

        protected int GetRegisterNumber(IToken rtoken)
        { // convert "rN" -> N
            String rs = rtoken.Text;
            rs = rs.Substring(1);
            return int.Parse(rs);
        }

        /// <summary>
        /// After parser is complete, look for unresolved labels
        /// </summary>
        protected override void CheckForUnresolvedReferences()
        {
            foreach (string name in labels.Keys)
            {
                LabelSymbol sym = (LabelSymbol)labels[name];
                if (!sym.IsDefined)
                {
                    Console.WriteLine("unresolved reference: " + name);
                }
            }
        }

        /// <summary>
        /// Compute the code address of a label
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected int GetLabelAddress(string id)
        {
            LabelSymbol? sym = null;

            if (labels.ContainsKey(id))
            {
                sym = labels[id];
            }

            if (sym == null)
            {
                // assume it's a forward code reference; record opnd address
                sym = new LabelSymbol(id, ip, true);
                sym.IsDefined = false;
                labels.Add(id, sym); // add to symbol table
            }
            else
            {
                if (sym.IsForwardRef)
                {
                    // address is unknown, must simply add to forward ref list
                    // record where in code memory we should patch later
                    sym.AddForwardReference(ip);
                }
                else
                {
                    // all is well; it's defined--just grab address
                    return sym.Address;
                }
            }

            return 0; // we don't know the real address yet
        }

        protected override void DefineFunction(IToken idToken, int args, int locals)
        {
            string name = idToken.Text;
            FunctionSymbol f = new FunctionSymbol(name, args, locals, ip);
            if (name.Equals("main")) mainFunction = f;
            // Did someone referred to this function before it was defined?
            // if so, replace element in constant pool (at same index)
            if (constPool.Contains(f)) constPool[constPool.IndexOf(f)] = f;
            else GetConstantPoolIndex(f); // save into constant pool
        }

        protected int GetFunctionIndex(string id)
        {
            int i = constPool.IndexOf(new FunctionSymbol(id));
            if (i >= 0) return i; // already in system; return index.
                                  // must be a forward function reference
                                  // create the constant pool entry; we'll fill in later
            return GetConstantPoolIndex(new FunctionSymbol(id));
        }

        protected override void DefineDataSize(int n) { dataSize = n; }

        protected override void DefineLabel(IToken idToken)
        {
            string id = idToken.Text;

            LabelSymbol? sym = null;
            if (labels.ContainsKey(id))
            {
                sym = (LabelSymbol)labels[id];
            }
             
            if (sym == null)
            {
                LabelSymbol csym = new LabelSymbol(id, ip, false);
                labels.Add(id, csym); // add to symbol table
            }
            else
            {
                if (sym.IsForwardRef)
                {
                    // we have found definition of forward
                    sym.IsDefined = true;
                    sym.Address = ip;
                    sym.ResolveForwardReferences(code);
                }
                else
                {
                    // redefinition of symbol
                    Console.WriteLine("line " + idToken.Line +
                            ": redefinition of symbol " + id);
                }
            }
        }

        protected void EnsureCapacity(int index)
        {
            if (index >= code.Length)
            { // expand
                int newSize = Math.Max(index, code.Length) * 2;
                byte[] bigger = new byte[newSize];
                Array.Copy(code, 0, bigger, 0, code.Length);
                code = bigger;
            }
        }

        public static int GetInt(byte[] memory, int index)
        {
            int b1 = memory[index++] & 0xFF; // mask off sign-extended bits
            int b2 = memory[index++] & 0xFF;
            int b3 = memory[index++] & 0xFF;
            int b4 = memory[index++] & 0xFF;
            int word = b1 << (8 * 3) | b2 << (8 * 2) | b3 << (8 * 1) | b4;
            return word;
        }

        /// <summary>
        /// Write value at index into a byte array highest to lowest byte, left to right.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public static void WriteInt(byte[] bytes, int index, int value)
        {
            bytes[index + 0] = (byte)((value >> (8 * 3)) & 0xFF); // get highest byte
            bytes[index + 1] = (byte)((value >> (8 * 2)) & 0xFF);
            bytes[index + 2] = (byte)((value >> (8 * 1)) & 0xFF);
            bytes[index + 3] = (byte)(value & 0xFF);
        }
    }
}
