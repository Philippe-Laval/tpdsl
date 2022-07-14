using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    public class Interpreter
    {
        /// <summary>
        /// default response to messages
        /// </summary>
        public IInterpreterListener Listener { get; set; } = new InterpreterListener();

        public GlobalScope GlobalScope { get; set; } = new GlobalScope();   // global scope is filled by the PieDefinitionVisitor
        
        IParseTree root = null!;               // the AST represents our code memory
        CommonTokenStream tokens = null!;       // was TokenRewriteStream
        PieLexer lex = null!;              // lexer/parser are part of the processor
        PieParser parser = null!;

        public Interpreter()
        {
           
        }

        public void Interp(TextReader input)
        {
            lex = new PieLexer(new AntlrInputStream(input));
            tokens = new CommonTokenStream(lex);    // was TokenRewriteStream
            parser = new PieParser(tokens, this);

            // Begin parsing at rule program
            PieParser.ProgramContext r = parser.program();

            Console.WriteLine(r.ToStringTree(parser));

            if (parser.NumberOfSyntaxErrors == 0)
            {
                root = (IParseTree)r;
                // print LISP-style tree
                //Console.WriteLine("tree: "+root.ToStringTree(parser));

                PieDefinitionVisitor definitionVisitor = new PieDefinitionVisitor(this);
                definitionVisitor.Visit(root);

                PieRuntimeVisitor runtimeVisitor = new PieRuntimeVisitor(this, definitionVisitor.Scopes);
                runtimeVisitor.Visit(root);
            }
        }


    }
}
