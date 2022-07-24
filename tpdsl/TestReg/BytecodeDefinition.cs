/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestReg
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
        public const short INSTR_BRF = 16;    // branch if false
        public const short INSTR_CCONST = 17;  // load constant char
        public const short INSTR_ICONST = 18;  // load constant integer
        public const short INSTR_FCONST = 19;  // load constant float
        public const short INSTR_SCONST = 20;  // load constant string
        public const short INSTR_GLOAD = 21;   // load from global memory
        public const short INSTR_GSTORE = 22;  // store in global memory
        public const short INSTR_FLOAD = 23;   // field load
        public const short INSTR_FSTORE = 24;  // store field
        public const short INSTR_MOVE = 25;   // reg to reg move
        public const short INSTR_PRINT = 26;   // print reg
        public const short INSTR_STRUCT = 27; // create new struct
        public const short INSTR_NULL = 28;   // load null into register
        public const short INSTR_HALT = 29;

        public static Instruction[] Instructions = new Instruction[]
        {
            null!, // <INVALID>
            new Instruction("iadd", REG,REG,REG), // index is the opcode
            new Instruction("isub", REG,REG,REG),
            new Instruction("imul", REG,REG,REG),
            new Instruction("ilt", REG,REG,REG),
            new Instruction("ieq", REG,REG,REG),
            new Instruction("fadd", REG,REG,REG),
            new Instruction("fsub", REG,REG,REG),
            new Instruction("fmul", REG,REG,REG),
            new Instruction("flt", REG,REG,REG),
            new Instruction("feq", REG,REG,REG),
            new Instruction("itof", REG,REG),
            new Instruction("call", FUNC,REG),
            new Instruction("ret"),
            new Instruction("br", INT),
            new Instruction("brt", REG,INT),
            new Instruction("brf", REG,INT),
            new Instruction("cconst", REG,INT),
            new Instruction("iconst", REG,INT),
            new Instruction("fconst", REG,POOL),
            new Instruction("sconst", REG,POOL),
            new Instruction("gload", REG,INT),
            new Instruction("gstore", REG,INT),
            new Instruction("fload", REG,REG,INT),
            new Instruction("fstore", REG,REG,INT),
            new Instruction("move", REG,REG),
            new Instruction("print", REG),
            new Instruction("struct", REG,INT),
            new Instruction("null", REG),
            new Instruction("halt")
        };

    }
}
