using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;

namespace TestNameListWithAssign
{
    class Program
    {
        static void Main(string[] args)
        {

            Test("Input1.txt");
        }

        private static void Test(string fileName)
        {
            using TextReader text_reader = File.OpenText(fileName);

            // Create an input character stream from standard in
            var input = new AntlrInputStream(text_reader);
            // Create an ExprLexer that feeds from that stream
            NameListWithAssignLexer lexer = new NameListWithAssignLexer(input);
            // Create a stream of tokens fed by the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            // Create a parser that feeds off the token stream
            NameListWithAssignParser parser = new NameListWithAssignParser(tokens);

            parser.RemoveErrorListeners(); // remove ConsoleErrorListener
            parser.AddErrorListener(new UnderlineListener()); // add ours
            //parser.AddErrorListener(new DiagnosticErrorListener());

            // Here's how to make the parser report all ambiguities
            //parser.Interpreter.PredictionMode = PredictionMode.SLL;
            parser.Interpreter.PredictionMode = PredictionMode.LL_EXACT_AMBIG_DETECTION;

            // Begin parsing at file rule
            IParseTree tree = parser.list();

            Console.WriteLine(tree.ToStringTree(parser)); // print LISP-style tree
        }
    }
}