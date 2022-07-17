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
