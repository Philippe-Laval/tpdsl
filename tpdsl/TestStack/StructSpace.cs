using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStack
{
    public class StructSpace
    {
        public object[] Fields { get; set; }
        
        public StructSpace(int nfields) 
        {
            Fields = new object[nfields];
        }

        public override string ToString()
        { 
            return string.Join(", ", Fields);
        }
    }
}
