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

namespace TestAggr
{
    public interface IScope
    {
        public string GetScopeName();

        /// <summary>
        /// Where to look next for symbols
        /// </summary>
        /// <returns></returns>
        public IScope? GetEnclosingScope();

        /// <summary>
        /// Define a symbol in the current scope
        /// </summary>
        /// <param name="sym"></param>
        public void Define(Symbol sym);

        /// <summary>
        /// Look up name in this scope or in enclosing scope if not here
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Symbol? Resolve(string name);
    }
}
