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
    public class StructSymbol : ScopedSymbol, IType, IScope
    {
        public Dictionary<string, Symbol> Fields { get; set; } = new Dictionary<String, Symbol>();

        public StructSymbol(string name, IScope? parent)
            : base(name, parent)
        { 
        }

        /// <summary>
        /// For a.b, only look in fields to resolve b, not up scope tree
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Symbol? ResolveMember(String name)
        {
            if (Fields.ContainsKey(name))
            {
                return Fields[name];
            }

            return null;
        }

        public override Dictionary<string, Symbol> GetMembers()
        {
            return Fields;
        }

        public override string ToString()
        {
            var temp = Fields.Select(o => $"{o.Key}={o.Value.ToString()}").ToList();
            var result = String.Join(", ", temp);

            return $"struct {this.GetName()}:{{{result}}}";
        }

    }
}


