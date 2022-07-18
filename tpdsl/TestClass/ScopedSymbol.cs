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
    public abstract class ScopedSymbol : Symbol, IScope
    {
        public IScope? _enclosingScope;

        public ScopedSymbol(string name, IType type, IScope? enclosingScope)
                : base(name, type)
        {
            _enclosingScope = enclosingScope;
        }

        public ScopedSymbol(string name, IScope? enclosingScope)
                : base(name)
        {
            _enclosingScope = enclosingScope;
        }

        public Symbol? Resolve(string name)
        {
            Symbol? s = null;

            Dictionary<string, Symbol> members = GetMembers();
            if (members.ContainsKey(name))
            {
                s = members[name];
            }

            if (s != null) return s;

            // if not here, check any enclosing scope
            if (GetParentScope() != null)
            {
                return GetParentScope()?.Resolve(name);
            }

            return null; // not found
        }

        //public Symbol? ResolveType(string name)
        //{
        //    return Resolve(name);
        //}

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

        public string GetScopeName() 
        { 
            return this.GetName();
        }

        /// <summary>
        /// Indicate how subclasses store scope members. 
        /// Allows us to factor out common code in this class.
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, Symbol> GetMembers();
 
    }
}




