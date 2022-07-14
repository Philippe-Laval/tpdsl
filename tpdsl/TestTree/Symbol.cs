using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    /// <summary>
    /// A generic programming language symbol
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// The symbol name
        /// </summary>
        protected string _name;

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
            String s = "";
            if (Scope != null) s = Scope.GetScopeName() + ".";
            return s + GetName();
        }
    }
}
