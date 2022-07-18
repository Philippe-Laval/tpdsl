using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace TestAggr
{
    public class DefPhase : CymbolBaseListener
    {
        public ParseTreeProperty<IScope> scopes { get; set; } = new ParseTreeProperty<IScope>();
        public SymbolTable symbolTable;
        private IScope? currentScope = null;     // define symbols in this scope


        public DefPhase(SymbolTable symtab)
        {
            symbolTable = symtab;
        }

        public override void EnterCompilationUnit(CymbolParser.CompilationUnitContext ctx)
        {
            currentScope = symbolTable.Globals;
        }

        public override void ExitCompilationUnit(CymbolParser.CompilationUnitContext ctx)
        {
            Console.WriteLine(symbolTable.Globals.ToString());
        }

        public override void EnterMethodDeclaration(CymbolParser.MethodDeclarationContext ctx)
        {
            String name = ctx.ID().GetText();
            string typeName = ctx.type().GetText();
            Symbol? type = currentScope?.Resolve(typeName);
            if (type is not null && type is BuiltInTypeSymbol builtInTypeSymbol)
            {
                // push new scope by making new one that points to enclosing scope
                MethodSymbol method = new MethodSymbol(name, builtInTypeSymbol, currentScope);
                currentScope?.Define(method); // Define function in current scope
                SaveScope(ctx, method);      // Push: set function's parent to current
                currentScope = method;       // Current scope is now function scope
            }
        }

        private void SaveScope(ParserRuleContext ctx, IScope s)
        {
            scopes.Put(ctx, s);
        }

        public override void ExitMethodDeclaration(CymbolParser.MethodDeclarationContext ctx)
        {
            Console.WriteLine(currentScope?.ToString());
            currentScope = currentScope?.GetEnclosingScope(); // pop scope
        }

        public override void EnterBlock(CymbolParser.BlockContext ctx)
        {
            // push new local scope
            currentScope = new LocalScope(currentScope);
            SaveScope(ctx, currentScope);
        }

        public override void ExitBlock(CymbolParser.BlockContext ctx)
        {
            if (currentScope is not null && currentScope is LocalScope localScope)
            {
                Console.WriteLine(localScope.ToString());
            }

            currentScope = currentScope?.GetEnclosingScope(); // pop scope
        }

        public override void ExitFormalParameter(CymbolParser.FormalParameterContext ctx)
        {
            DefineVar(ctx.type(), ctx.ID().Symbol);
        }

        public override void ExitVarDeclaration(CymbolParser.VarDeclarationContext ctx)
        { 
            DefineVar(ctx.type(), ctx.ID().Symbol);
        }

        private void DefineVar(CymbolParser.TypeContext typeCtx, IToken nameToken)
        {
            string typeName = typeCtx.GetText();
            Symbol? type = currentScope?.Resolve(typeName);
            if (type is not null && type is BuiltInTypeSymbol builtInTypeSymbol)
            {
                VariableSymbol var = new VariableSymbol(nameToken.Text, builtInTypeSymbol);
                currentScope?.Define(var); // Define symbol in current scope
            }
        }
    }
}
