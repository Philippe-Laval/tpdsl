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
    public class DisAssembler
    {
        byte[] code;
        int codeSize;
        protected object[] constPool;
        //BytecodeDefinition def;

        public DisAssembler(byte[] code,
                            int codeSize,
                            object[] constPool)
        {
            this.code = code;
            this.codeSize = codeSize;
            this.constPool = constPool;
        }

        public void Disassemble()
        {
            Console.WriteLine("Disassembly:");
            int i = 0;
            while (i < codeSize)
            {
                i = DisassembleInstruction(i);
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public int DisassembleInstruction(int ip)
        {
            int opcode = code[ip];
            Instruction I = BytecodeDefinition.Instructions[opcode];
            string instrName = I.Name;
            //Console.Write("%04d:\t%-11s", ip, instrName
            Console.Write($"{ip}:\t{instrName}");
            ip++;
            if (I.N == 0)
            {
                Console.Write("  ");
                return ip;
            }
            List<String> operands = new List<string>();
            for (int i = 0; i < I.N; i++)
            {
                int opnd = BytecodeAssembler.GetInt(code, ip);
                ip += 4;
                switch (I.Type[i])
                {
                    case BytecodeDefinition.REG:
                        operands.Add("r" + opnd);
                        break;
                    case BytecodeDefinition.FUNC:
                    case BytecodeDefinition.POOL:
                        operands.Add(ShowConstPoolOperand(opnd));
                        break;
                    case BytecodeDefinition.INT:
                        operands.Add(opnd.ToString());
                        break;
                }
            }
            for (int i = 0; i < operands.Count; i++)
            {
                string s = (string)operands[i];
                if (i > 0) Console.Write(", ");
                Console.Write(s);
            }
            return ip;
        }

        private string ShowConstPoolOperand(int poolIndex)
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("#");
            buf.Append(poolIndex);
            var myConst = constPool[poolIndex];
            if (myConst != null)
            {
                string s = myConst.ToString() ?? "";
                if (myConst is string)
                {
                    s = '"' + s + '"';
                }
                else if (myConst is FunctionSymbol)
                {
                    FunctionSymbol fs = (FunctionSymbol)constPool[poolIndex];
                    s = fs.Name + "()@" + fs.Address;
                }

                buf.Append(":");
                buf.Append(s);
            }
            return buf.ToString();
        }
    }
}
