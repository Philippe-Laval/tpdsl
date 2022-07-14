using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStack
{
    public class BytecodeDefinition
    {
        // operand types
        public const int REG = AssemblerParser.REG;
        public const int FUNC = AssemblerParser.FUNC;
        public const int INT = AssemblerParser.INT;
        public const int POOL = 1000; // unique imaginary token

        // INSTRUCTION BYTECODES (byte is signed; use a short to keep 0..255)
        public const short INSTR_IADD = 1;    // int add
        public const short INSTR_ISUB = 2;
        public const short INSTR_IMUL = 3;
        public const short INSTR_ILT = 4;     // int less than
        public const short INSTR_IEQ = 5;     // int equal
        public const short INSTR_FADD = 6;    // float add
        public const short INSTR_FSUB = 7;
        public const short INSTR_FMUL = 8;
        public const short INSTR_FLT = 9;     // float less than
        public const short INSTR_FEQ = 10;
        public const short INSTR_ITOF = 11;   // int to float
        public const short INSTR_CALL = 12;
        public const short INSTR_RET = 13;    // return with/without value
        public const short INSTR_BR = 14;     // branch
        public const short INSTR_BRT = 15;    // branch if true
        public const short INSTR_BRF = 16;    // branch if true
        public const short INSTR_CCONST = 17; // push constant char
        public const short INSTR_ICONST = 18; // push constant integer
        public const short INSTR_FCONST = 19; // push constant float
        public const short INSTR_SCONST = 20; // push constant string
        public const short INSTR_LOAD = 21;   // load from local context
        public const short INSTR_GLOAD = 22;  // load from global memory
        public const short INSTR_FLOAD = 23;  // field load
        public const short INSTR_STORE = 24;  // storein local context
        public const short INSTR_GSTORE = 25; // store in global memory
        public const short INSTR_FSTORE = 26; // field store
        public const short INSTR_PRINT = 27;  // print stack top
        public const short INSTR_STRUCT = 28; // push new struct on stack
        public const short INSTR_NULL = 29;   // push null onto stack
        public const short INSTR_POP = 30;    // throw away top of stack
        public const short INSTR_HALT = 31;

        public static Instruction[] Instructions = new Instruction[] {
            null!, // <INVALID>
            new Instruction("iadd"), // index is the opcode
            new Instruction("isub"),
            new Instruction("imul"),
            new Instruction("ilt"),
            new Instruction("ieq"),
            new Instruction("fadd"),
            new Instruction("fsub"),
            new Instruction("fmul"),
            new Instruction("flt"),
            new Instruction("feq"),
            new Instruction("itof"),
            new Instruction("call", FUNC),
            new Instruction("ret"),
            new Instruction("br", INT),
            new Instruction("brt", INT),
            new Instruction("brf", INT),
            new Instruction("cconst", INT),
            new Instruction("iconst", INT),
            new Instruction("fconst", POOL),
            new Instruction("sconst", POOL),
            new Instruction("load", INT),
            new Instruction("gload", INT),
            new Instruction("fload", INT),
            new Instruction("store", INT),
            new Instruction("gstore", INT),
            new Instruction("fstore", INT),
            new Instruction("print"),
            new Instruction("struct", INT),
            new Instruction("null"),
            new Instruction("pop"),
            new Instruction("halt")
        };
    }
}
