using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    /// <summary>
    /// A function invocation scope; stores arguments and locals
    /// </summary>
    public class FunctionSpace : MemorySpace
    {
        /// <summary>
        /// what function are we executing?
        /// </summary>
        public FunctionSymbol Def { get; set; }

        public FunctionSpace(FunctionSymbol functionSymbol) 
            : base(functionSymbol.GetName() + " invocation")
        {
            Def = functionSymbol;
        }
    
    }
}
