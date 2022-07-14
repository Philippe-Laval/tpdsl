using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNested
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
        /// Create a Symbol object
        /// </summary>
        /// <param name="name">the symbol name</param>
        public Symbol(string name) 
        {
            _name = name;
        }

        public Symbol(String name, IType? type) 
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
            if (Type != null) return '<' + GetName() + ":" + Type + '>';
            return GetName();
        }
    }
}
