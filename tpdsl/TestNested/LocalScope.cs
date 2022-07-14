using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNested
{
    internal class LocalScope : BaseScope
    {
        public LocalScope(IScope? parent) 
            : base(parent)
        {  
        }

        public override string GetScopeName() 
        {
            return "local";
        }    
    
    }
}
