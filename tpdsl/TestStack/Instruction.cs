using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStack
{
    public class Instruction
    {
        public string Name { get; set; } // E.g., "iadd", "call"
        public int[] Type { get; set; } = new int[3];
        public int N { get; set; } = 0;
        
        public Instruction(string name)
        : this(name, 0, 0, 0) 
        {
            N = 0;
        }
        
        public Instruction(string name, int a)
            : this(name, a, 0, 0)
        {
            N = 1;
        }

        public Instruction(string name, int a, int b) 
            : this(name, a, b, 0)
        {
            N = 2;
        }

        public Instruction(string name, int a, int b, int c)
        {
            this.Name = name;
            Type[0] = a;
            Type[1] = b;
            Type[2] = c;
            N = 3;
        }
    }
}
