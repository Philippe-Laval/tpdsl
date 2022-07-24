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
    public class FunctionSymbol
    {
        public string Name { get; set; }
        public int Nargs { get; set; } // how many arguments are there?
        public int Nlocals { get; set; } // how many locals are there?
        public int Address { get; set; }

        public FunctionSymbol(string name)
        {
            Name = name;
        }

        public FunctionSymbol(string name, int nargs, int nlocals, int address)
        {
            Name = name;
            Nargs = nargs;
            Nlocals = nlocals;
            Address = address;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(Object? o)
        {
            return o is FunctionSymbol && Name.Equals(((FunctionSymbol)o).Name);
        }

        public override string ToString()
        {
            return "FunctionSymbol{" +
                   "name='" + Name + '\'' +
                   ", args=" + Nargs +
                   ", locals=" + Nlocals +
                   ", address=" + Address +
                   '}';
        }

    }
}
