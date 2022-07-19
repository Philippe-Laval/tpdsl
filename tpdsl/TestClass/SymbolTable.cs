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

namespace TestClass
{
    public class SymbolTable
    {
        public GlobalScope Globals { get; set; } = new GlobalScope();
        //ClassSymbol objectRoot;

        public SymbolTable() 
        {
            InitTypeSystem();
        }

        protected void InitTypeSystem()
        {
            // if you wanted a predefined Object class hierarchy root
            // like Java, you'd define it here:
            /*
                    objectRoot = new ClassSymbol("Object", globals, null);
                    MethodSymbol hashCode =
                       new MethodSymbol("hashCode",new BuiltInTypeSymbol("int"),objectRoot);
                    objectRoot.define(hashCode);
                    globals.define(objectRoot);
            */

            // define predefined atomic types
            Globals.Define(new BuiltInTypeSymbol("int"));
            Globals.Define(new BuiltInTypeSymbol("float"));
            Globals.Define(new BuiltInTypeSymbol("void")); // pseudo-type
        }

        /*
        public static Symbol? ResolveID(CymbolAST idAST)
        {
            Symbol s = idAST.scope.resolve(idAST.getText());
            Console.WriteLine("line " + idAST.getLine() + ": resolve " +
                               idAST.getText() + " to " + s);
            if (s.Def == null) return s; // must be predefined symbol
                                         // if resolves to local or global symbol, token index of definition
                                         // must be before token index of reference
            int idLocation = idAST.token.getTokenIndex();
            int defLocation = s.Def.token.getTokenIndex();
            if (idAST.scope is BaseScope &&
                 s.Scope is BaseScope &&
                 idLocation < defLocation )
        {
                Console.WriteLine("line " + idAST.getLine() +
                    ": error: forward local var ref " + idAST.getText());
                return null;
            }
            return s;
        }
        */

        /// <summary>
        /// 'this' and 'super' need to know about enclosing class
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ClassSymbol? GetEnclosingClass(IScope? s)
        {
            while (s != null)
            { 
                // walk upwards from s looking for a class
                if (s is ClassSymbol) return (ClassSymbol)s;
                
                s = s.GetParentScope();
            }
            return null;
        }

        public override string ToString() 
        { 
            return Globals.ToString(); 
        }
    }
}
