using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonolithic
{
    /// <summary>
    /// A symbol for built in types such int, float primitive types
    /// </summary>
    public class BuiltInTypeSymbol : Symbol, IType
    {
        public BuiltInTypeSymbol(string name) 
            : base(name)
        {
        }
    }
}
