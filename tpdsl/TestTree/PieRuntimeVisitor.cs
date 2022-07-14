using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    public class PieRuntimeVisitor : PieBaseVisitor<object?>
    {
        public readonly ReturnValue SharedReturnValue = new ReturnValue();

        public Interpreter Interp { get; set; }
        private ParseTreeProperty<IScope?> _scopes;
        //IScope currentScope;

        private MemorySpace _globals = new MemorySpace("globals");           // global memory
        private MemorySpace _currentSpace;
        private Stack<FunctionSpace> _stack = new Stack<FunctionSpace>();    // call stack

        public PieRuntimeVisitor(Interpreter interp, ParseTreeProperty<IScope?> scopes)
        {
            Interp = interp;
            //currentScope = interp.GlobalScope;
            _scopes = scopes;

            _currentSpace = _globals;
        }

        public override object? VisitProgram(PieParser.ProgramContext context)
        {
            foreach (PieParser.StatementContext statementContext in context.statement())
            {
                Visit(statementContext);
            }

            return null;
        }

        public override object? VisitInstance([NotNull] PieParser.InstanceContext context)
        {
            // 'new' sname=ID

            IScope? scope = _scopes.Get(context);

            StructSymbol? structSymbol = (StructSymbol?)scope?.Resolve(context.sname.Text);
            if (structSymbol != null)
            {
                return new StructInstance(structSymbol);
            }

            return null;
        }

        public override object? VisitQid(PieParser.QidContext context)
        {
            if (context.ID().Length > 1)
            {
                return FieldLoad(context);
            }

            // Load variable
            return VariableLoad(context);
        }

        public override object? VisitAssignementStatement(PieParser.AssignementStatementContext context)
        {
            // qid '=' expr NL

            var qid = context.qid();   // get operands
            var expr = context.expr();

            object? value = Visit(expr);            // walk/evaluate expr

            if (qid.ChildCount > 1)
            {
                FieldAssign(qid, value); 
                return null;
            }

            // var assign
            string variableName = qid.ID(0).GetText();
            MemorySpace? space = GetSpaceWithSymbol(variableName);
            if (space == null)
            {
                space = _currentSpace; // create in current space
            }
            space.Put(variableName, value);         // store

            return null;
        }

        public void FieldAssign(PieParser.QidContext qid, object? value)
        {
            //string all = qid.GetText();
            //string objectToAssign = qid.ID(0).GetText();
            //List<string> parts = new List<string>();
            //for (int index = 1; index < qid.ID().Length; index++)
            //{
            //    string part = qid.ID(index).GetText();
            //    parts.Add(part);
            //}
            //string allAccessors = string.Join(".", parts);

            ITerminalNode o = qid.ID(0);
            ITerminalNode f = qid.ID(1);
            String fieldname = f.GetText();
            object? a = VariableLoad(qid);
            if (a is StructInstance structInstance)
            {
                if (structInstance?.Def.ResolveMember(fieldname) == null)
                {
                    Interp.Listener.Error("can't assign; " + structInstance?.Name + " has no " + fieldname + " field", f.Symbol);
                    return;
                }

                structInstance.Put(fieldname, value);
            }
            else
            {
                // make a good error message:
                string all = qid.GetText();
                string leftpart = qid.ID(0).GetText();

                Interp.Listener.Error(leftpart + " is not a struct in " + all, qid.ID(0).Symbol);
                return;
            }
        }

        /// <summary>
        /// Return scope holding id's value; current func space or global.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MemorySpace? GetSpaceWithSymbol(string id)
        {
            // If we have called some functions (we have some FunctionSpace objects on the stack)
            if (_stack.Count > 0)
            {
                // Get the FunctionSpace object of the last called function
                FunctionSpace functionSpace = _stack.Peek();
                // Check if the symbol is a parameter of the function
                if (functionSpace.ContainsMember(id))
                {
                    // in top stack?
                    return functionSpace;
                }
            }

            // Search in globals?
            if (_globals.ContainsMember(id)) return _globals;

            // nowhere
            return null;
        }

        public object? VariableLoad(PieParser.QidContext context)
        {
            IScope? scope = _scopes.Get(context);

            // Load variable
            string variableName = context.ID(0).GetText();
            MemorySpace? s = GetSpaceWithSymbol(variableName); // just a not a.b
            if (s != null) return s?.Get(variableName);
            Interp.Listener.Error("no such variable " + variableName, context.ID(0).Symbol);
            return null;
        }

        public object? FieldLoad(PieParser.QidContext context)
        {    //  a.b
            var expr = context.ID(0); // get left node or subtree
            var b = context.ID(1);    // must be an ID node

            string variableName = expr.GetText();
            string id = b.GetText();
            
            StructInstance? structInstance = (StructInstance?)VariableLoad(context); // find expr
            if (structInstance?.Def.ResolveMember(id) == null)
            {
                // is it a struct?
                Interp.Listener.Error(structInstance?.Name + " has no " + id + " field", b.Symbol);
                return null;
            }

            return structInstance.Get(id);
        }

        public override object? VisitPrintStatement(PieParser.PrintStatementContext context)
        {
            // 'print' expr NL
            var result = Visit(context.expr());
            Console.WriteLine(result);

            return null;
        }

        public override object? VisitIfStatement(PieParser.IfStatementContext context)
        {
            // 'if' expr c=slist ('else' el=slist)?
            PieParser.ExprContext condStart = context.expr();
            PieParser.SlistContext codeStart = context.c;
            PieParser.SlistContext? elseCodeStart = null;
            
            if (context.ChildCount == 5)
            {
                elseCodeStart = context.el;
            }

            bool? c = (bool?) Visit(condStart);
            if (c.HasValue && c.Value)
            {
                return Visit(codeStart);
            }
            else
            {
                if (elseCodeStart != null)
                {
                    return Visit(elseCodeStart);
                }
            }

            return null;
        }

        public override object? VisitWhileStatement(PieParser.WhileStatementContext context)
        {
            // 'while' expr slist
            PieParser.ExprContext condStart = context.expr();
            PieParser.SlistContext codeStart = context.slist();

            bool? c = (bool?)Visit(condStart);
            while (c.HasValue && c.Value)
            {
                Visit(codeStart);
                c = (bool?)Visit(condStart);
            }

            return null;
        }

        public override object? VisitReturnStatement(PieParser.ReturnStatementContext context)
        {
            // 'return' expr NL
            SharedReturnValue.Value = Visit(context.expr());
            throw SharedReturnValue;
        }

        public override object? VisitIntAtom(PieParser.IntAtomContext context)
        {
            if (context.INT() is { } i)
                return int.Parse(i.GetText());

            return null;
        }

        public override object? VisitFloatAtom(PieParser.FloatAtomContext context)
        {
            if (context.FLOAT() is { } f)
                return float.Parse(f.GetText(), CultureInfo.InvariantCulture);

            return null;
        }

        public override object? VisitCharAtom(PieParser.CharAtomContext context)
        {
            if (context.CHAR() is { } c)
                return char.Parse(c.GetText()[1..^1]);

            return null;
        }

        public override object? VisitStringAtom(PieParser.StringAtomContext context)
        {
            if (context.STRING() is { } s)
                return s.GetText()[1..^1];

            return null;
        }

        public override object? VisitParenthesizedAtom(PieParser.ParenthesizedAtomContext context)
        {
            return Visit(context.expr());
        }


        public override object? VisitExpr(PieParser.ExprContext context)
        {
            // addexpr (compOp addexpr)?

            if (context.addexpr().Length == 1)
            {
                return Visit(context.addexpr(0));
            }
            else if (context.addexpr().Length == 2)
            {
                var left = Visit(context.addexpr(0));
                var right = Visit(context.addexpr(1));
                var op = context.compOp().GetText();

                var result = op switch
                {
                    "==" => IsEquals(left, right),
                    "<" => LessThan(left, right),
                    _ => throw new NotImplementedException()
                };

                return result;
            }

            throw new NotImplementedException();
        }

        public override object? VisitAddexpr(PieParser.AddexprContext context)
        {
            // mulexpr (addOp mulexpr)*

            if (context.mulexpr().Length == 1)
            {
                return Visit(context.mulexpr(0));
            }

            var accumulator = Visit(context.mulexpr(0));

            for (int index = 0; index < context.addOp().Length; index++)
            {
                var right = Visit(context.mulexpr(index+1));
                var op = context.addOp(index).GetText();

                accumulator = op switch
                {
                    "+" => Add(accumulator, right),
                    "-" => Subtract(accumulator, right),
                    _ => throw new NotImplementedException()
                };
            }

            return accumulator;
        }

        public override object? VisitMulexpr([NotNull] PieParser.MulexprContext context)
        {
            // atom (multOp atom)*

            if (context.atom().Length == 1)
            {
                return Visit(context.atom(0));
            }

            var accumulator = Visit(context.atom(0));

            for (int index = 0; index < context.multOp().Length; index++)
            {
                var right = Visit(context.atom(index + 1));
                var op = context.multOp(index).GetText();

                accumulator = op switch
                {
                    "*" => Mult(accumulator, right),
                    "/" => Div(accumulator, right),
                    _ => throw new NotImplementedException()
                };
            }

            return accumulator;
        }


        private bool IsEquals(object? left, object? right)
        {
            if (left is string sl && right is string sr)
                return sl == sr;

            if (left is char cl && right is char cr)
                return cl == cr;

            if (left is int l && right is int r)
                return l == r;

            if (left is float lf && right is float rf)
                return lf == rf;

            if (left is int lInt && right is float rFloat)
                return lInt == rFloat;

            if (left is float lFloat && right is float rInt)
                return lFloat == rInt;

            throw new Exception($"IsEquals : Cannot compare values of type {left?.GetType()} and {right?.GetType()}.");
        }

        private bool LessThan(object? left, object? right)
        {
            var rt = right?.GetType();
            var lt = left?.GetType();

            if (left is char cl && right is char cr)
                return cl < cr;

            if (left is int l && right is int r)
                return l < r;

            if (left is float lf && right is float rf)
                return lf < rf;

            if (left is int lInt && right is float rFloat)
                return lInt < rFloat;

            if (left is float lFloat && right is float rInt)
                return lFloat < rInt;

            throw new Exception($"LessThan : Cannot compare values of type {left?.GetType()} and {right?.GetType()}.");
        }


        private object? Add(object? left, object? right)
        {
            if (left is int l && right is int r)
                return l + r;

            if (left is float lf && right is float rf)
                return lf + rf;

            if (left is int lInt && right is float rFloat)
                return lInt + rFloat;

            if (left is float lFloat && right is int rInt)
                return lFloat + rInt;

            if (left is string)
                return $"{left}{right}";

            if (right is string)
                return $"{left}{right}";

            throw new Exception($"Cannot add values of type {left?.GetType()} and {right?.GetType()}.");
        }

        private object? Subtract(object? left, object? right)
        {
            if (left is int l && right is int r)
                return l - r;

            if (left is float lf && right is float rf)
                return lf - rf;

            if (left is int lInt && right is float rFloat)
                return lInt - rFloat;

            if (left is float lFloat && right is int rInt)
                return lFloat - rInt;

            throw new NotImplementedException();
        }

        private object? Mult(object? left, object? right)
        {
            if (left is int l && right is int r)
                return l * r;

            if (left is float lf && right is float rf)
                return lf * rf;

            if (left is int lInt && right is float rFloat)
                return lInt * rFloat;

            if (left is float lFloat && right is int rInt)
                return lFloat * rInt;

            throw new NotImplementedException();
        }

        private object? Div(object? left, object? right)
        {
            if (left is int l && right is int r)
                return l / r;

            if (left is float lf && right is float rf)
                return lf / rf;

            if (left is int lInt && right is float rFloat)
                return lInt / rFloat;

            if (left is float lFloat && right is int rInt)
                return lFloat / rInt;

            throw new NotImplementedException();
        }

        public override object? VisitCall([NotNull] PieParser.CallContext context)
        {
            // name=ID '(' (expr (',' expr )*)? ')'

            // Resolve function's name
            string fname = context.ID().GetText() + "()";

            IScope? scope = _scopes.Get(context);

            FunctionSymbol? fs = (FunctionSymbol?)scope?.Resolve(fname);

            if (fs == null)
            {
                Interp.Listener.Error("no such function " + fname, context.name);
                return null;
            }

            FunctionSpace fspace = new FunctionSpace(fs);
            MemorySpace saveSpace = _currentSpace;
            _currentSpace = fspace;

            // check for argument compatibility
            int argCount = context.expr().Length;
            if (fs.FormalArgs == null && argCount > 0 || // args compatible?
                 fs.FormalArgs != null && fs.FormalArgs.Count != argCount)
            {
                Interp.Listener.Error("function " + fs.GetName() + " argument list mismatch");
                return null;
            }

            int i = 0; // define args according to order in formalArgs
            foreach (Symbol argS in fs.FormalArgs.Values)
            {
                VariableSymbol arg = (VariableSymbol)argS;
                PieParser.ExprContext ithArg = context.expr(i);
                object? argValue = Visit(ithArg);
                fspace.Put(arg.GetName(), argValue);
                i++;
            }

            object? result = null;
            _stack.Push(fspace);        // PUSH new arg, local scope
            try { Visit(fs.BlockAST); } // do the call
            catch (ReturnValue rv) { result = rv.Value; } // trap return value
            _stack.Pop();               // POP arg, locals
            _currentSpace = saveSpace;
            return result;
        }

    }
}
