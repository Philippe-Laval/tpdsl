using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace TestAggr
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

        public override void EnterMethodDeclaration(CymbolParser.MethodDeclarationContext ctx)
        {
            currentScope = scopes.Get(ctx);
        }
        public override void ExitMethodDeclaration(CymbolParser.MethodDeclarationContext ctx)
        {
            currentScope = currentScope?.GetEnclosingScope();
        }

        public override void EnterBlock(CymbolParser.BlockContext ctx)
        {
            currentScope = scopes.Get(ctx);
        }
        public override void ExitBlock(CymbolParser.BlockContext ctx)
        {
            currentScope = currentScope?.GetEnclosingScope();
        }

        public override void ExitVar(CymbolParser.VarContext ctx)
        {
            String name = ctx.qid().ID(0).Symbol.Text;
            Symbol? variable = currentScope?.Resolve(name);
            if (variable == null)
            {
                Program.Error(ctx.qid().ID(0).Symbol, "no such variable: " + name);
            }
            if (variable is MethodSymbol)
            {
                Program.Error(ctx.qid().ID(0).Symbol, name + " is not a variable");
            }
        }

        public override void ExitCall(CymbolParser.CallContext ctx)
        {
            // can only handle f(...) not expr(...)
            String funcName = ctx.ID().GetText();
            Symbol? meth = currentScope?.Resolve(funcName);
            if (meth == null)
            {
                Program.Error(ctx.ID().Symbol, "no such function: " + funcName);
            }
            if (meth is VariableSymbol)
            {
                Program.Error(ctx.ID().Symbol, funcName + " is not a function");
            }
        }

    }
}
