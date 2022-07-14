using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNested
{
    internal class VariableSymbol : Symbol
    {
        public VariableSymbol(string name, IType? type) 
            : base(name, type) 
        {  
        }

    }
}
