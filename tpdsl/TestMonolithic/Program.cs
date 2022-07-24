using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;

namespace TestMonolithic
{
    class Program
    {
        static void Main(string[] args)
        {
            Test("t.cymbol");
        }

        private static void Test(string fileName)
        {
            using TextReader text_reader = File.OpenText(fileName);

            // Create an input character stream from standard in
            var input = new AntlrInputStream(text_reader);
            // Create an ExprLexer that feeds from that stream
            CymbolLexer lexer = new CymbolLexer(input);
            // Create a stream of tokens fed by the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            // Create a parser that feeds off the token stream
            CymbolParser parser = new CymbolParser(tokens);

            parser.RemoveErrorListeners(); // remove ConsoleErrorListener
            parser.AddErrorListener(new UnderlineListener()); // add ours
            //parser.AddErrorListener(new DiagnosticErrorListener());

            // Here's how to make the parser report all ambiguities
            //parser.Interpreter.PredictionMode = PredictionMode.SLL;
            parser.Interpreter.PredictionMode = PredictionMode.LL_EXACT_AMBIG_DETECTION;

            SymbolTable symtab = new SymbolTable();    // create symbol table
            

            // Begin parsing at stat rule
            IParseTree tree = parser.compilationUnit(symtab);

            Console.WriteLine(tree.ToStringTree(parser)); // print LISP-style tree

            Console.WriteLine($"symtab: {symtab.ToString()}");
        }
    }
}