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

namespace TestClass
{
    /// <summary>
    /// A generic programming language symbol
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// The symbol's name
        /// </summary>
        protected string _name;

        /// <summary>
        /// The symbol's type
        /// </summary>
        public IType? Type { get; set; } = null;

        /// <summary>
        /// The scope where the symbol is defined
        /// </summary>
        public IScope? Scope { get; set; } = null;

        /// <summary>
        /// points at ID node in tree
        /// </summary>
        // public SlistContext Def { get; set; } = null!;

        /// <summary>
        /// Create a Symbol object
        /// </summary>
        /// <param name="name">the symbol name</param>
        public Symbol(string name) 
        {
            _name = name;
        }

        public Symbol(string name, IType? type) 
            : this(name)
        {
            Type = type; 
        }

        /// <summary>
        /// Get the symbol name
        /// </summary>
        /// <returns></returns>
        public virtual string GetName()
        {
            return _name;
        }

        public override string ToString()
        {
            string s = string.Empty;
            if (Scope != null) s = Scope.GetScopeName() + ".";
            if (Type != null) return '<' + s + GetName() + ":" + Type + '>';
            return s + GetName();
        }
    }
}
