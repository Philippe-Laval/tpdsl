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
    /// A simple register-based interpreter
    /// </summary>
    internal class Interpreter
    {
        public const int DEFAULT_OPERAND_STACK_SIZE = 100;
        public const int DEFAULT_CALL_STACK_SIZE = 1000;

        DisAssembler disasm = null!;

        int ip;             // instruction pointer register
        byte[] code = null!;        // byte-addressable code memory.
        int codeSize;
        object[] globals = null!;   // global variable space
        protected object[] constPool = null!;
        /** Stack of stack frames, grows upwards */
        StackFrame[] calls = new StackFrame[DEFAULT_CALL_STACK_SIZE];
        int fp = -1;        // frame pointer register
        FunctionSymbol mainFunction = null!;

        public bool TraceEnabled { get; set; } = false;

        public bool Load(Interpreter interp, TextReader input)
        {
            bool hasErrors = false;
            try
            {
                var inputSream = new AntlrInputStream(input);
                AssemblerLexer assemblerLexer = new AssemblerLexer(inputSream);
                CommonTokenStream tokens = new CommonTokenStream(assemblerLexer);
                BytecodeAssembler assembler = new BytecodeAssembler(tokens, BytecodeDefinition.Instructions);
                assembler.program();
                interp.code = assembler.GetMachineCode();
                interp.codeSize = assembler.GetCodeMemorySize();
                interp.constPool = assembler.GetConstantPool();
                interp.mainFunction = assembler.GetMainFunction();
                interp.globals = new object[assembler.GetDataSize()];
                interp.disasm = new DisAssembler(interp.code,
                                                 interp.codeSize,
                                                 interp.constPool);
                // hasErrors = assembler.GetNumberOfSyntaxErrors() > 0;
                hasErrors = false;
            }
            finally
            {
                input.Close();
            }
            return hasErrors;
        }

        /** Execute the bytecodes in code memory starting at mainAddr */
        public void Exec()
        {
            // SIMULATE "call main()"; set up stack as if we'd called main()
            if (mainFunction == null)
            {
                mainFunction = new FunctionSymbol("main", 0, 0, 0);
            }
            StackFrame f = new StackFrame(mainFunction, ip);
            calls[++fp] = f;
            ip = mainFunction.Address;
            Cpu();
        }

        /** Simulate the fetch-execute cycle */
        protected void Cpu()
        {
            int i = 0, j = 0, k = 0, addr = 0, fieldIndex = 0;
            short opcode = code[ip];
            while (opcode != BytecodeDefinition.INSTR_HALT && ip < codeSize)
            {
                if (TraceEnabled) Trace();
                ip++; //jump to next instruction or first byte of operand
                var r = calls[fp].Registers; // shortcut to current registers
                switch (opcode)
                {
                    case BytecodeDefinition.INSTR_IADD:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        k = GetRegOperand();
                        r[k] = ((int)r[i]) + ((int)r[j]);
                        break;
                    // ...
                    case BytecodeDefinition.INSTR_ISUB:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        k = GetRegOperand();
                        r[k] = ((int)r[i]) - ((int)r[j]);
                        break;
                    case BytecodeDefinition.INSTR_IMUL:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        k = GetRegOperand();
                        r[k] = ((int)r[i]) * ((int)r[j]);
                        break;
                    case BytecodeDefinition.INSTR_ILT:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        k = GetRegOperand();
                        r[k] = ((int)r[i]) < ((int)r[j]);
                        break;
                    case BytecodeDefinition.INSTR_IEQ:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        k = GetRegOperand();
                        r[k] = ((int)r[i]) == ((int)r[j]);
                        break;
                    case BytecodeDefinition.INSTR_FADD:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        k = GetRegOperand();
                        r[k] = ((float)r[i]) + ((float)r[j]);
                        break;
                    case BytecodeDefinition.INSTR_FSUB:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        k = GetRegOperand();
                        r[k] = ((float)r[i]) - ((float)r[j]);
                        break;
                    case BytecodeDefinition.INSTR_FMUL:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        k = GetRegOperand();
                        r[k] = ((float)r[i]) * ((float)r[j]);
                        break;
                    case BytecodeDefinition.INSTR_FLT:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        k = GetRegOperand();
                        r[k] = ((float)r[i]) < ((float)r[j]);
                        break;
                    case BytecodeDefinition.INSTR_FEQ:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        k = GetRegOperand();
                        r[k] = ((float)r[i]) == ((float)r[j]);
                        break;
                    case BytecodeDefinition.INSTR_ITOF:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        r[j] = (float)((int)r[i]);
                        break;
                    case BytecodeDefinition.INSTR_CALL:
                        int funcStringIndex = GetIntOperand();
                        int baseRegisterIndex = GetRegOperand();
                        Call(funcStringIndex, baseRegisterIndex);
                        break;
                    case BytecodeDefinition.INSTR_RET:
                        StackFrame f = calls[fp--]; // pop stack frame
                        calls[fp].Registers[0] = f.Registers[0];
                        ip = f.ReturnAddress;
                        break;
                    case BytecodeDefinition.INSTR_BR:
                        ip = GetIntOperand();
                        break;
                    case BytecodeDefinition.INSTR_BRT:
                        i = GetRegOperand();
                        addr = GetIntOperand();
                        Boolean bv = (Boolean)r[i];
                        if (bv) ip = addr;
                        break;
                    case BytecodeDefinition.INSTR_BRF:
                        i = GetRegOperand();
                        addr = GetIntOperand();
                        Boolean bv2 = (Boolean)r[i];
                        if (!bv2) ip = addr;
                        break;
                    case BytecodeDefinition.INSTR_CCONST:
                        i = GetRegOperand();
                        r[i] = (char)GetIntOperand();
                        break;
                    case BytecodeDefinition.INSTR_ICONST:
                        i = GetRegOperand();
                        r[i] = GetIntOperand();
                        break;
                    case BytecodeDefinition.INSTR_FCONST:
                    case BytecodeDefinition.INSTR_SCONST:
                        i = GetRegOperand();
                        int constIndex = GetIntOperand();
                        r[i] = constPool[constIndex];
                        break;
                    case BytecodeDefinition.INSTR_GLOAD:
                        i = GetRegOperand();
                        addr = GetIntOperand();
                        r[i] = globals[addr];
                        break;
                    case BytecodeDefinition.INSTR_GSTORE:
                        i = GetRegOperand();
                        addr = GetIntOperand();
                        globals[addr] = r[i];
                        break;
                    case BytecodeDefinition.INSTR_FLOAD:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        fieldIndex = GetRegOperand();
                        r[i] = ((StructSpace)r[j]).Fields[fieldIndex];
                        break;
                    case BytecodeDefinition.INSTR_FSTORE:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        fieldIndex = GetRegOperand();
                        ((StructSpace)r[j]).Fields[fieldIndex] = r[i];
                        break;
                    case BytecodeDefinition.INSTR_MOVE:
                        i = GetRegOperand();
                        j = GetRegOperand();
                        r[j] = r[i];
                        break;
                    case BytecodeDefinition.INSTR_PRINT:
                        i = GetRegOperand();
                        Console.WriteLine(r[i]);
                        break;
                    case BytecodeDefinition.INSTR_STRUCT:
                        i = GetRegOperand();
                        int nfields = GetIntOperand();
                        r[i] = new StructSpace(nfields);
                        break;
                    case BytecodeDefinition.INSTR_NULL:
                        i = GetRegOperand();
                        r[i] = null!;
                        break;
                    default:
                        throw new Exception("invalid opcode: " + opcode + " at ip=" + (ip - 1));
                }
                opcode = code[ip];
            }
        }

        protected void Call(int functionConstPoolIndex, int baseRegisterIndex)
        {
            FunctionSymbol fs = (FunctionSymbol)constPool[functionConstPoolIndex];
            StackFrame f = new StackFrame(fs, ip);
            StackFrame callingFrame = calls[fp];
            calls[++fp] = f; // push new stack frame
                             // move arguments from calling stack frame to new stack frame
            for (int a = 0; a < fs.Nargs; a++)
            { // move args, leaving room for r0
                f.Registers[a + 1] = callingFrame.Registers[baseRegisterIndex + a];
            }
            ip = fs.Address; // branch to function
        }

        /** Pull off 4 bytes starting at ip and return 32-bit signed int value.
         *  Return with ip pointing *after* last byte of operand.  The byte-order
         *  is high byte down to low byte, left to right.
         */
        protected int GetIntOperand()
        {
            int word = BytecodeAssembler.GetInt(code, ip);
            ip += 4;
            return word;
        }

        protected int GetRegOperand()
        {
            return GetIntOperand();
        }

        // Tracing, dumping, ...

        public void Disassemble()
        {
            disasm.Disassemble();
        }

        protected void Trace()
        {
            disasm.DisassembleInstruction(ip);
            var r = calls[fp].Registers;
            if (r.Length > 0)
            {
                Console.Write("\t" + calls[fp].Sym.Name + ".registers=[");
                ;
                for (int i = 0; i < r.Length; i++)
                {
                    if (i == 1) Console.Write(" |");
                    if (i == calls[fp].Sym.Nargs + 1 && i == 1) Console.Write("|");
                    else if (i == calls[fp].Sym.Nargs + 1) Console.Write(" |");
                    Console.Write(" ");
                    if (r[i] == null) Console.Write("?");
                    else Console.Write(r[i]);
                }
                Console.Write(" ]");
            }
            if (fp >= 0)
            {
                Console.Write("  calls=[");
                for (int i = 0; i <= fp; i++)
                {
                    Console.Write(" " + calls[i].Sym.Name);
                }
                Console.Write(" ]");
            }
            Console.WriteLine();
        }

        public void Coredump()
        {
            if (constPool.Length > 0) DumpConstantPool();
            if (globals.Length > 0) DumpDataMemory();
            DumpCodeMemory();
        }

        protected void DumpConstantPool()
        {
            Console.WriteLine("Constant pool:");
            int addr = 0;
            foreach (object o in constPool)
            {
                if (o is string)
                {
                    //Console.Write("%04d: \"%s\"\n", addr, o);
                    Console.Write($"{addr}: \"{o}\"\n");
                }
                else
                {
                    //Console.Write("%04d: %s\n", addr, o);
                    Console.Write($"{addr}: {o}\n");
                }
                addr++;
            }
            Console.WriteLine();
        }

        protected void DumpDataMemory()
        {
            Console.WriteLine("Data memory:");
            int addr = 0;
            foreach (object o in globals)
            {
                if (o != null)
                {
                    //Console.Write("%04d: %s <%s>\n", addr, o, o.GetType().FullName);
                    Console.Write($"{addr}: {o} <{o.GetType().FullName}>\n", addr, o, o.GetType().FullName);
                }
                else
                {
                    //Console.Write("%04d: <null>\n", addr);
                    Console.Write($"{addr}: <null>\n");
                }
                addr++;
            }
            Console.WriteLine();
        }

        public void DumpCodeMemory()
        {
            Console.WriteLine("Code memory:");
            for (int i = 0; code != null && i < codeSize; i++)
            {
                if (i % 8 == 0 && i != 0) Console.WriteLine();
                //if (i % 8 == 0) Console.Write("%04d:", i);
                if (i % 8 == 0) Console.Write($"{i}:");
                //Console.Write(" %3d", code[i]);
                Console.Write($" {code[i]}");
            }
            Console.WriteLine();
        }
    }
}
