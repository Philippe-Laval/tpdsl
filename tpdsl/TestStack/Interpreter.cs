using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Antlr4.Runtime;

namespace TestStack
{
    /// <summary>
    /// A simple stack-based interpreter
    /// </summary>
    public class Interpreter
    {
        public const int DEFAULT_OPERAND_STACK_SIZE = 100;
        public const int DEFAULT_CALL_STACK_SIZE = 1000;

        DisAssembler disasm = null!;

        int ip;             // instruction pointer register
        byte[] code = null!;        // byte-addressable code memory.
        int codeSize;
        object[] globals = null!;   // global variable space
        protected object[] constPool = null!;

        /** Operand stack, grows upwards */
        object[] _operands = new object[DEFAULT_OPERAND_STACK_SIZE];
        int sp = -1;        // stack pointer register

        /** Stack of stack frames, grows upwards */
        StackFrame[] calls = new StackFrame[DEFAULT_CALL_STACK_SIZE];
        int fp = -1;        // frame pointer register
        FunctionSymbol mainFunction = null!;

        public bool TraceEnabled { get; set; } = false;

        public Interpreter()
        {
        }

        public bool Load(TextReader input)
        {
            bool hasErrors = false;
            try
            {
                AssemblerLexer assemblerLexer =
                    new AssemblerLexer(new AntlrInputStream(input));
                CommonTokenStream tokens = new CommonTokenStream(assemblerLexer);
                BytecodeAssembler assembler = new BytecodeAssembler(tokens, BytecodeDefinition.Instructions);
                assembler.program();
                this.code = assembler.GetMachineCode();
                this.codeSize = assembler.GetCodeMemorySize();
                this.constPool = assembler.GetConstantPool();
                this.mainFunction = assembler.GetMainFunction();
                this.globals = new Object[assembler.GetDataSize()];
                this.disasm = new DisAssembler(this.code,
                                             this.codeSize,
                                             this.constPool);
                //hasErrors = assembler.GetNumberOfSyntaxErrors() > 0;
                hasErrors = false;
            }
            finally
            {
                input.Close();
            }
            return hasErrors;
        }

        /// <summary>
        /// Execute the bytecodes in code memory starting at mainAddr
        /// </summary>
        public void Exec()
        {
            // SIMULATE "call main()"; set up stack as if we'd called main()
            if (mainFunction == null)
            {
                mainFunction = new FunctionSymbol("main", 0, 0, 0);
            }
            StackFrame f = new StackFrame(mainFunction, -1);
            calls[++fp] = f;
            ip = mainFunction.Address;
            Cpu();
        }


        /// <summary>
        /// Simulate the fetch-execute cycle
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected void Cpu()
        {
            object? v = null; // some locals to reuse
            int a, b;
            float e, f;
            int addr = 0;
            short opcode = code[ip];
            while (opcode != BytecodeDefinition.INSTR_HALT && ip < codeSize)
            {
                if (TraceEnabled) InterpreterTrace();
                ip++; //jump to next instruction or first byte of operand
                switch (opcode)
                {
                    case BytecodeDefinition.INSTR_IADD:
                        a = (int)_operands[sp - 1]; // 1st opnd 1 below top
                        b = (int)_operands[sp];     // 2nd opnd at top of stack
                        sp -= 2;                    // pop both operands
                        _operands[++sp] = a + b;    // push result
                        break;
                    case BytecodeDefinition.INSTR_ISUB:
                        a = (int)_operands[sp - 1];
                        b = (int)_operands[sp];
                        sp -= 2;
                        _operands[++sp] = a - b;
                        break;
                    case BytecodeDefinition.INSTR_IMUL:
                        a = (int)_operands[sp - 1];
                        b = (int)_operands[sp];
                        sp -= 2;
                        _operands[++sp] = a * b;
                        break;
                    case BytecodeDefinition.INSTR_ILT:
                        a = (int)_operands[sp - 1];
                        b = (int)_operands[sp];
                        sp -= 2;
                        _operands[++sp] = a < b;
                        break;
                    case BytecodeDefinition.INSTR_IEQ:
                        a = (int)_operands[sp - 1];
                        b = (int)_operands[sp];
                        sp -= 2;
                        _operands[++sp] = a == b;
                        break;
                    case BytecodeDefinition.INSTR_FADD:
                        e = (float)_operands[sp - 1];
                        f = (float)_operands[sp];
                        sp -= 2;
                        _operands[++sp] = e + f;
                        break;
                    case BytecodeDefinition.INSTR_FSUB:
                        e = (float)_operands[sp - 1];
                        f = (float)_operands[sp];
                        sp -= 2;
                        _operands[++sp] = e - f;
                        break;
                    case BytecodeDefinition.INSTR_FMUL:
                        e = (float)_operands[sp - 1];
                        f = (float)_operands[sp];
                        sp -= 2;
                        _operands[++sp] = e * f;
                        break;
                    case BytecodeDefinition.INSTR_FLT:
                        e = (float)_operands[sp - 1];
                        f = (float)_operands[sp];
                        sp -= 2;
                        _operands[++sp] = e < f;
                        break;
                    case BytecodeDefinition.INSTR_FEQ:
                        e = (float)_operands[sp - 1];
                        f = (float)_operands[sp];
                        sp -= 2;
                        _operands[++sp] = e == f;
                        break;
                    case BytecodeDefinition.INSTR_ITOF:
                        a = (int)_operands[sp--];
                        _operands[++sp] = (float)a;
                        break;
                    case BytecodeDefinition.INSTR_CALL:
                        int funcIndexInConstPool = GetIntOperand();
                        Call(funcIndexInConstPool);
                        break;
                    case BytecodeDefinition.INSTR_RET:  // result is on op stack
                        StackFrame fr = calls[fp--];    // pop stack frame
                        ip = fr.ReturnAddress;          // branch to ret addr
                        break;
                    case BytecodeDefinition.INSTR_BR:
                        ip = GetIntOperand();
                        break;
                    case BytecodeDefinition.INSTR_BRT:
                        addr = GetIntOperand();
                        if (_operands[sp--].Equals(true)) ip = addr;
                        break;
                    case BytecodeDefinition.INSTR_BRF:
                        addr = GetIntOperand();
                        if (_operands[sp--].Equals(false)) ip = addr;
                        break;
                    case BytecodeDefinition.INSTR_CCONST:
                        _operands[++sp] = (char)GetIntOperand();
                        break;
                    case BytecodeDefinition.INSTR_ICONST:
                        _operands[++sp] = GetIntOperand(); // push operand
                        break;
                    case BytecodeDefinition.INSTR_FCONST:
                    case BytecodeDefinition.INSTR_SCONST:
                        int constPoolIndex = GetIntOperand();
                        _operands[++sp] = constPool[constPoolIndex];
                        break;
                    case BytecodeDefinition.INSTR_LOAD: // load from call stack
                        addr = GetIntOperand();
                        _operands[++sp] = calls[fp].Locals[addr];
                        break;
                    case BytecodeDefinition.INSTR_GLOAD:// load from global memory
                        addr = GetIntOperand();
                        _operands[++sp] = globals[addr];
                        break;
                    case BytecodeDefinition.INSTR_FLOAD:
                        StructSpace structSpace = (StructSpace)_operands[sp--];
                        int fieldOffset = GetIntOperand();
                        _operands[++sp] = structSpace.Fields[fieldOffset];
                        break;
                    case BytecodeDefinition.INSTR_STORE:
                        addr = GetIntOperand();
                        calls[fp].Locals[addr] = _operands[sp--];
                        break;
                    case BytecodeDefinition.INSTR_GSTORE:
                        addr = GetIntOperand();
                        globals[addr] = _operands[sp--];
                        break;
                    case BytecodeDefinition.INSTR_FSTORE:
                        structSpace = (StructSpace)_operands[sp--];
                        v = _operands[sp--];
                        fieldOffset = GetIntOperand();
                        structSpace.Fields[fieldOffset] = v;
                        break;
                    case BytecodeDefinition.INSTR_PRINT:
                        Console.WriteLine(_operands[sp--]);
                        break;
                    case BytecodeDefinition.INSTR_STRUCT:
                        int nfields = GetIntOperand();
                        _operands[++sp] = new StructSpace(nfields);
                        break;
                    case BytecodeDefinition.INSTR_NULL:
                        _operands[++sp] = null!;
                        break;
                    case BytecodeDefinition.INSTR_POP:
                        --sp;
                        break;
                    default:
                        throw new Exception("invalid opcode: " + opcode + " at ip=" + (ip - 1));
                }
                opcode = code[ip];
            }
        }
       

        protected void Call(int functionConstPoolIndex)
        {
            FunctionSymbol fs = (FunctionSymbol)constPool[functionConstPoolIndex];
            StackFrame f = new StackFrame(fs, ip);
            calls[++fp] = f; // push new stack frame for parameters and locals
                             // move args from operand stack to top frame on call stack
            for (int a = fs.Nargs - 1; a >= 0; a--)
            {
                f.Locals[a] = _operands[sp--];
            }
            ip = fs.Address; // branch to function
        }

        /// <summary>
        /// Pull off 4 bytes starting at ip and return 32-bit signed int value.
        ///  Return with ip pointing *after* last byte of operand.The byte-order
        ///  is high byte down to low byte, left to right.
        /// </summary>
        /// <returns></returns>
        protected int GetIntOperand()
        {
            int word = BytecodeAssembler.GetInt(code, ip);
            ip += 4;
            return word;
        }

        // Tracing, dumping, ...

        public void Disassemble()
        {
            disasm.Disassemble();
        }

        protected void InterpreterTrace()
        {
            disasm.DisassembleInstruction(ip);
            Console.Write("\tstack=[");
            for (int i = 0; i <= sp; i++)
            {
                Object o = _operands[i];
                Console.Write(" " + o);
            }
            Console.Write(" ]");
            if (fp >= 0)
            {
                Console.Write(", calls=[");
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
                if (o is String)
                {
                    Console.Write("%04d: \"%s\"\n", addr, o);
                }
                else
                {
                    Console.Write("%04d: %s\n", addr, o);
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
                    Console.Write("%04d: %s <%s>\n",
                                  addr, o, o.GetType().FullName);
                }
                else
                {
                    Console.Write("%04d: <null>\n", addr);
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
                if (i % 8 == 0) Console.Write("%04d:", i);
                Console.Write(" %3d", ((int)code[i]));
            }
            Console.WriteLine();
        }
    }
}
