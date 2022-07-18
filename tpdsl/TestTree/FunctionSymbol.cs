using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestTree.PieParser;

namespace TestTree
{
    public class FunctionSymbol : ScopedSymbol
    {
        public Dictionary<string, Symbol> FormalArgs { get; set; } = new Dictionary<string, Symbol>();
        public SlistContext BlockAST { get; set; } = null!;

        public FunctionSymbol(string name, IScope? parent) 
            : base(name, parent)
        {
        }

        public override Dictionary<string, Symbol> GetMembers()
        { 
            return FormalArgs;
        }

        public override string GetName()
        {
            var temp = FormalArgs.Select(o => $"{o.Key}").ToList();
            var result = string.Join(", ", temp);

            return _name + "(" + result + ")";
        }
    
    }
}
