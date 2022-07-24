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
        public object[] Registers { get; set; }

        public StackFrame(FunctionSymbol sym, int returnAddress)
        {
            Sym = sym;
            ReturnAddress = returnAddress;
            // Allocate space for registers; 1 extra for r0 reserved reg
            Registers = new object[sym.Nargs + sym.Nlocals + 1];
        }
    }
}
