using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    public class StructSymbol : ScopedSymbol, IScope
    {
        public Dictionary<string, Symbol> Fields { get; set; } = new Dictionary<string, Symbol>();
        
        public StructSymbol(string name, IScope? parent)
            : base(name, parent)
        {
        }

        /// <summary>
        /// For a.b, only look in a only to resolve b, not up scope tree
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Symbol? ResolveMember(string name)
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
            var temp = Fields.Select(o => $"{o.Key}={o.Value.ToString()}");
            var result = String.Join(", ", temp);

            return $"struct {this.GetName()}:{result}";
        }
    
    }
}
