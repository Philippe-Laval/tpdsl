using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;

namespace TestNested
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1("t.cymbol");
            Test1("t2.cymbol");
        }

        private static void Test1(string fileName)
        {
            using TextReader text_reader = File.OpenText(fileName);

            // Create an input character stream from standard in
            var input = new AntlrInputStream(text_reader);
            // Create an ExprLexer that feeds from that stream
            CymbolLexer lexer = new CymbolLexer(input);
            // Create a stream of tokens fed by the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);

            SymbolTable symtab = new SymbolTable(); // make global scope, types

            // Create a parser that feeds off the token stream
            CymbolParser parser = new CymbolParser(tokens);

            parser.RemoveErrorListeners(); // remove ConsoleErrorListener
            parser.AddErrorListener(new UnderlineListener()); // add ours
            //parser.AddErrorListener(new DiagnosticErrorListener());

            // Here's how to make the parser report all ambiguities
            //parser.Interpreter.PredictionMode = PredictionMode.Sll;
            parser.Interpreter.PredictionMode = PredictionMode.LlExactAmbigDetection;

            // Begin parsing at file rule
            IParseTree tree = parser.compilationUnit();

            Console.WriteLine(tree.ToStringTree(parser)); // print LISP-style tree
        }
    }
}