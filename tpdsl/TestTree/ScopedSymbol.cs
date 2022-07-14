using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    public abstract class ScopedSymbol : Symbol, IScope
    {
        public IScope? _enclosingScope;

        public ScopedSymbol(string name, IScope? enclosingScope)
            : base(name)
        {
            _enclosingScope = enclosingScope;
        }

        public Symbol? Resolve(String name)
        {
            Symbol? s = null;

            Dictionary<string, Symbol> members = GetMembers();
            if (members.ContainsKey(name))
            {
                s = members[name];
            }
            
            if (s != null) return s;
            
            // if not here, check any parent scope
            if (GetParentScope() != null)
            {
                return GetParentScope()?.Resolve(name);
            }
            
            return null; // not found
        }

        public void Define(Symbol sym)
        {
            var name = sym.GetName();
            sym.Scope = this; // track the scope in each symbol

            Dictionary<string, Symbol> members = GetMembers();
            if (!members.ContainsKey(name))
            {
                members.Add(name, sym);
            }
        }

        public IScope? GetParentScope()
        {
            return GetEnclosingScope();
        }

        public IScope? GetEnclosingScope()
        {
            return _enclosingScope;
        }

        public String GetScopeName() 
        { 
            return this.GetName();
        }

        /// <summary>
        /// Indicate how subclasses store scope members. 
        /// Allows us object factor out common code in this class.
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, Symbol> GetMembers();

    }
}
