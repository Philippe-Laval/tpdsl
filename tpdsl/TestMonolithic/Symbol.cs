using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonolithic
{
    /// <summary>
    /// A generic programming language symbol
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// All symbols at least have a name
        /// </summary>
        public string Name { get; set; }
        public IType? Type { get; set; } = null;

        public Symbol(string name)
        {
            Name = name;
        }

        public Symbol(string name, IType? type)
            : this(name)
        {
            Type = type;
        }

        public override string ToString()
        {
            if (Type != null) return $"<{Name}:{Type}>";
            return Name;
        }
    }
}
