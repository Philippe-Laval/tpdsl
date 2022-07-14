using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonolithic
{
    public class SymbolTable : IScope
    {
        /// <summary>
        /// single-scope symtab
        /// </summary>
        public Dictionary<string, Symbol> Symbols { get; set; } = new Dictionary<string, Symbol>();
        
        public SymbolTable()
        {
            InitTypeSystem();
        }
        
        protected void InitTypeSystem()
        {
            Define(new BuiltInTypeSymbol("int"));
            Define(new BuiltInTypeSymbol("float"));
        }

        #region Implement IScope interface

        public string GetScopeName()
        {
            return "global"; 
        }
        
        public IScope? GetEnclosingScope() 
        { 
            return null;
        }

        public void Define(Symbol sym)
        {
            if (!Symbols.ContainsKey(sym.Name))
            {
                Symbols.Add(sym.Name, sym);
            }
        }
        
        public Symbol? Resolve(string name)
        {
            if (Symbols.ContainsKey(name))
            {
                return Symbols[name];
            }

            return null;
        }
        
        #endregion

        public override string ToString() 
        {
            string symbols = string.Join(", ", Symbols.Select(o => $"{o.Key}={o.Value.ToString()}"));
            return $"{GetScopeName()}:{{{symbols}}}"; 
        }
    
    }
}
