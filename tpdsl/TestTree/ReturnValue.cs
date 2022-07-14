using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    public class ReturnValue : Exception
    {
        public object? Value { get; set; }
        
        public ReturnValue() 
            : base()
        {
        }
    }
}
