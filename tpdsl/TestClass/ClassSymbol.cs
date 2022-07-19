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
    public class ClassSymbol : ScopedSymbol, IScope, IType
    {
        /// <summary>
        /// This is the superclass not enclosingScope field. We still record
        ///  the enclosing scope so we can push in and pop out of class defs.
        /// </summary>
        public ClassSymbol SuperClass { get; set; }
        
        /// <summary>
        /// List of all fields and methods
        /// </summary>
        public Dictionary<string, Symbol> Members { get; set; } = new Dictionary<string, Symbol>();

        public ClassSymbol(String name, IScope enclosingScope, ClassSymbol superClass)
                : base(name, enclosingScope)
        {
            this.SuperClass = superClass;
        }

        public override IScope? GetParentScope()
        {
            if (SuperClass == null) return EnclosingScope; // globals
            return SuperClass; // if not root object, return super
        }

        /// <summary>
        /// For a.b, only look in a's class hierarchy to resolve b, not globals
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Symbol? ResolveMember(String name)
        {
            Symbol? s = null;

            if (Members.ContainsKey(name))
            {
                s = Members[name];
            }

            if (s != null) return s;

            // if not here, check just the superclass chain
            if (SuperClass != null)
            {
                return SuperClass.ResolveMember(name);
            }

            return null; // not found
        }

        public override Dictionary<string, Symbol> GetMembers()
        {
            return Members;
        }

        public override string ToString()
        {
            var temp = Members.Select(o => $"{o.Key}").ToList();
            var membersResult = string.Join(", ", temp);

            string result = "class " + GetName() + ":{" + membersResult + "}";
            return result;
        }
    }
}




