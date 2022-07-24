using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;

namespace TestSyntax
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1("t.q");
            Test1("t2.q");
            Test1("t3.q");
            Test1("t4.q");
            Test1("t5.q");

            // Test2("t.q");
        }

        private static void Test1(string fileName)
        {
            using TextReader input = File.OpenText(fileName);

            Interpreter interp = new Interpreter();
            interp.Interp(input);
        }

        private static void Test2(string fileName)
        {
            using TextReader text_reader = File.OpenText(fileName);

            // Create an input character stream from standard in
            var input = new AntlrInputStream(text_reader);
            // Create an ExprLexer that feeds from that stream
            QLexer lexer = new QLexer(input);
            // Create a stream of tokens fed by the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);

            Interpreter interp = new Interpreter();

            // Create a parser that feeds off the token stream
            QParser parser = new QParser(tokens, interp);

            parser.RemoveErrorListeners(); // remove ConsoleErrorListener
            parser.AddErrorListener(new UnderlineListener()); // add ours
            //parser.AddErrorListener(new DiagnosticErrorListener());

            // Here's how to make the parser report all ambiguities
            //parser.Interpreter.PredictionMode = PredictionMode.SLL;
            parser.Interpreter.PredictionMode = PredictionMode.LL_EXACT_AMBIG_DETECTION;

            // Begin parsing at file rule
            IParseTree tree = parser.program();

            Console.WriteLine(tree.ToStringTree(parser)); // print LISP-style tree
        }
    }
}