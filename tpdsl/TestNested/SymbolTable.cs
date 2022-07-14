using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNested
{
    public class SymbolTable
    {
        public GlobalScope Globals { get; set; } = new GlobalScope();

        public SymbolTable() 
        {
            InitTypeSystem();
        }

        protected void InitTypeSystem()
        {
            Globals.Define(new BuiltInTypeSymbol("int"));
            Globals.Define(new BuiltInTypeSymbol("float"));
            Globals.Define(new BuiltInTypeSymbol("void")); // pseudo-type
        }

        public override string ToString() 
        { 
            return Globals.ToString(); 
        }
    }
}
