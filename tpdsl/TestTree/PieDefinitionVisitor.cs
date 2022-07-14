using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    public class PieDefinitionVisitor : PieBaseVisitor<object?>
    {
        public Interpreter Interp { get; set; }
        IScope? currentScope;
        public ParseTreeProperty<IScope?> Scopes { get; set; } = new ParseTreeProperty<IScope?>();

        public PieDefinitionVisitor(Interpreter interp)
        {
            Interp = interp;
            currentScope = interp.GlobalScope;
        }

        private void SaveScope(ParserRuleContext ctx, IScope? s)
        {
            Scopes.Put(ctx, s);
        }

        public override object? VisitProgram([NotNull] PieParser.ProgramContext context)
        {
            Console.WriteLine($"Visit Program");

            Console.WriteLine($"Part1 : Visit Function Definitions ");

            foreach (PieParser.FunctionDefinitionContext functionDefinitionContext in context.functionDefinition())
            {
                Visit(functionDefinitionContext);
            }

            Console.WriteLine($"Part2 : Visit Statements ");

            foreach (PieParser.StatementContext statementContext in context.statement())
            {
                Visit(statementContext);
            }

            Console.WriteLine($"Global Scope");
            Console.WriteLine(Interp.GlobalScope.ToString());

            return null;
        }

        public override object? VisitFunctionDefinition([NotNull] PieParser.FunctionDefinitionContext context)
        {
            // 'def' ID '(' (vardef (',' vardef)* )? ')' slist

            Console.WriteLine($"Visit Function Definition {context.ID().GetText()}");

            var enumerable = context.vardef().Select(o => o.ID().GetText());
            string temp = String.Join(", ", enumerable);
            string myF = $"{context.ID().GetText()}({temp})";

            string functionName = context.ID().GetText();
            FunctionSymbol fs = new FunctionSymbol(functionName, currentScope);
            fs.BlockAST = context.slist();

            currentScope?.Define(fs); // def method in globals
            currentScope = fs;       // set current scope to method scope

            foreach (PieParser.VardefContext vardefContext in context.vardef())
            {
                Visit(vardefContext);
            }

            currentScope = new LocalScope(fs);

            Visit(context.slist());

            // Get out of LocalScope
            currentScope = currentScope?.GetEnclosingScope();

            // Get out of FunctionSymbol
            currentScope = currentScope?.GetEnclosingScope();

            return null;
        }

        public override object? VisitSlist([NotNull] PieParser.SlistContext context)
        {
            // ':' NL statement+ '.' NL | statement

            Console.WriteLine($"Visit Slist");

            foreach (var s in context.statement())
            {
                Visit(s);
            }

            return null;
        }


        public override object? VisitAssignementStatement(PieParser.AssignementStatementContext context)
        {
            // qid '=' expr NL

            Console.WriteLine($"Visit Assignement Statement {context.qid().GetText()}");
            // We can have : u.name.y => only take the left part "u"
            // u = new User
            // u.name = "parrt"    # make u.name a string
            // u.name.y = "parrt"  # u.name is a string not a struct

            SaveScope(context, currentScope);

            PieParser.QidContext qidContext = context.qid();

            VariableSymbol vs = new VariableSymbol(qidContext.ID(0).GetText());
            currentScope?.Define(vs);

            Visit(context.expr());

            return null;
        }

        //public override object? VisitQid(PieParser.QidContext context)
        //{
        //    // ID ('.' ID)*

        //    SaveScope(context, currentScope);
        //    VariableSymbol vs = new VariableSymbol(context.ID(0).GetText());
        //    currentScope?.Define(vs);

        //    return VisitChildren(context);
        //}

        public override object? VisitStructDefinition(PieParser.StructDefinitionContext context)
        {
            // 'struct' ID '{' vardef (',' vardef)* '}' NL

            Console.WriteLine($"Visit Struct Definition {context.ID().GetText()}");
 
            // Mettre ou pas ? SaveScope
            SaveScope(context, currentScope);

            StructSymbol ss = new StructSymbol(context.ID().GetText(), currentScope);
            currentScope?.Define(ss); // def struct in current scope
            currentScope = ss;       // set current scope to struct scope

            foreach (PieParser.VardefContext vardefContext in context.vardef())
            {
                Visit(vardefContext);
            }

            currentScope = currentScope?.GetEnclosingScope();

            return null;
        }

        public override object? VisitVardef(PieParser.VardefContext context)
        {
            // ID

            Console.WriteLine($"Visit Vardef {context.ID().GetText()}");

            SaveScope(context, currentScope);

            VariableSymbol vs = new VariableSymbol(context.ID().GetText());
            currentScope?.Define(vs);

            return null;
        }

        public override object? VisitInstance(PieParser.InstanceContext context)
        {
            // 'new' sname=ID

            Console.WriteLine($"Visit Instance {context.ID().GetText()}");

            SaveScope(context, currentScope);

            return null;
        }

        public override object? VisitCall([NotNull] PieParser.CallContext context)
        {
            // name=ID '(' (expr (',' expr )*)? ')'

            Console.WriteLine($"Visit Call {context.ID().GetText()}");

            SaveScope(context, currentScope);

            return null;
        }

    }
}
