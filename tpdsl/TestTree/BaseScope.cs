using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    public abstract class BaseScope : IScope
	{
		/// <summary>
		/// null if global (outermost) scope
		/// </summary>
		private IScope? _enclosingScope;
		public Dictionary<string, Symbol> Symbols { get; set; } = new Dictionary<String, Symbol>();

		public BaseScope(IScope? enclosingScope)
		{
			_enclosingScope = enclosingScope;
		}

		public Symbol? Resolve(String name)
		{
			Symbol? s = null;

			if (Symbols.ContainsKey(name))
			{
				s = Symbols[name];
			}

			if (s != null) return s;

			// if not here, check any enclosing scope
			IScope? parentScope = GetParentScope();
			if (parentScope != null)
			{
				return parentScope.Resolve(name);
			}

			return null; // not found
		}

		public void Define(Symbol sym)
		{
			var symbolName = sym.GetName();
			if (!Symbols.ContainsKey(symbolName))
			{
				Symbols.Add(symbolName, sym);
				sym.Scope = this; // track the scope in each symbol
			}
		}

		public abstract string GetScopeName();

		public IScope? GetParentScope() { return GetEnclosingScope(); }

		public IScope? GetEnclosingScope() { return _enclosingScope; }

		public override string ToString()
		{
			var temp = Symbols.Select(o => $"{o.Key}={o.Value.ToString()}").ToList();
			var result = String.Join(", ", temp);

			return result;
		}
    
    }
}
