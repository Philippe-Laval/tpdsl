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
	public class MethodSymbol : ScopedSymbol
	{
		public Dictionary<string, Symbol> OrderedArgs { get; set; } = new Dictionary<string, Symbol>();

		public MethodSymbol(string name, IType returnType, IScope? enclosingScope)
			: base(name, returnType, enclosingScope)
		{
		}

		public override Dictionary<string, Symbol> GetMembers()
		{ 
			return OrderedArgs; 
		}

		public override string GetName()
		{
			var temp = OrderedArgs.Select(o => $"{o.Key}").ToList();
			var result = string.Join(", ", temp);

			return _name + "(" + result + ")";
		}
	}
}
