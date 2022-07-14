using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNested
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
			if (_enclosingScope != null)
			{
				return _enclosingScope.Resolve(name);
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

		public IScope? GetEnclosingScope() { return _enclosingScope; }

		public override string ToString()
		{
			var temp = Symbols.Select(o => $"{o.Key}");
			var result = String.Join(", ", temp);

			return $"{GetScopeName()}:{result}";
		}
	}
}
