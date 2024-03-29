﻿/***
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

namespace TestNested
{
	public class MethodSymbol : Symbol, IScope
	{
		public Dictionary<string, Symbol> OrderedArgs { get; set; } = new Dictionary<string, Symbol>();
		private IScope? _enclosingScope;

		public MethodSymbol(string name, IType returnType, IScope? enclosingScope)
			: base(name, returnType)
		{
			_enclosingScope = enclosingScope;
		}

		public Symbol? Resolve(string name)
		{
			Symbol? s = null;

			if (OrderedArgs.ContainsKey(name))
			{
				s = OrderedArgs[name];
			}

			if (s != null) return s;

			// if not here, check any enclosing scope
			var scope = GetEnclosingScope();
			if (scope != null)
			{
				return scope.Resolve(name);
			}

			// not found
			return null;
		}

		public void Define(Symbol sym)
		{
			string name = sym.GetName();

			if (!OrderedArgs.ContainsKey(name))
			{
				OrderedArgs.Add(name, sym);
				sym.Scope = this; // track the scope in each symbol
			}
		}

		public IScope? GetEnclosingScope()
		{
			return _enclosingScope;
		}

		public string GetScopeName()
		{
			return _name;
		}

		public override string ToString()
		{
			var temp = OrderedArgs.Select(o => $"{o.Value.ToString()}").ToList();
			var paramResult = string.Join(", ", temp);

			string result = $"method{base.ToString()}:[{paramResult}]";
			return result;
		}
	}
}
