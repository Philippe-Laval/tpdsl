using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStack
{
    public class StackFrame
    {
        /// <summary>
        /// associated with which function?
        /// </summary>
        public FunctionSymbol Sym { get; set; }

        /// <summary>
        /// the instruction following the call
        /// </summary>
        public int ReturnAddress { get; set; }

        /// <summary>
        /// holds parameters and local variables
        /// </summary>
        public object[] Locals { get; set; }

        public StackFrame(FunctionSymbol sym, int returnAddress)
        {
            Sym = sym;
            ReturnAddress = returnAddress;
            Locals = new object[sym.Nargs + sym.Nlocals];
        }
    }
}
