using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;

namespace TestVecMath
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
            VecMathLexer lexer = new VecMathLexer(input);
            // Create a stream of tokens fed by the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            // Create a parser that feeds off the token stream
            VecMathParser parser = new VecMathParser(tokens);

            parser.RemoveErrorListeners(); // remove ConsoleErrorListener
            parser.AddErrorListener(new UnderlineListener()); // add ours
            //parser.AddErrorListener(new DiagnosticErrorListener());

            // Here's how to make the parser report all ambiguities
            //parser.Interpreter.PredictionMode = PredictionMode.Sll;
            parser.Interpreter.PredictionMode = PredictionMode.LlExactAmbigDetection;

            // Begin parsing at statlist rule
            IParseTree tree = parser.statlist();

            Console.WriteLine(tree.ToStringTree(parser)); // print LISP-style tree
        }
    }
}