using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace TestNested
{
    public class RefPhase : CymbolBaseListener
    {
        ParseTreeProperty<IScope> scopes;
        SymbolTable symtab;
        IScope? currentScope = null; // resolve symbols starting in this scope

        public RefPhase(SymbolTable symtab, ParseTreeProperty<IScope> scopes)
        {
            this.symtab = symtab;
            this.scopes = scopes;
        }
        public override void EnterCompilationUnit(CymbolParser.CompilationUnitContext ctx)
        {
            currentScope = symtab.Globals;
        }
    }
}
