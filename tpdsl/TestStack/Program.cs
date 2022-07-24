using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;

namespace TestStack
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] myargs = new string[] { "t.pcode" };

            Test2(myargs);
            //Test("t.pcode");
        }

        private static void Test2(string[] args)
        {
            // PROCESS ARGS
            bool trace = false;
            bool disassemble = false;
            bool dump = false;
            string? filename = null;

            int i = 0;
            while (i < args.Length)
            {
                if (args[i].Equals("-trace")) { trace = true; i++; }
                else if (args[i].Equals("-dis")) { disassemble = true; i++; }
                else if (args[i].Equals("-dump")) { dump = true; i++; }
                else { filename = args[i]; i++; }
            }

            TextReader input = null!;
            if (filename != null) 
                input = File.OpenText(filename);
            else 
                input = Console.In;

            Interpreter interpreter = new Interpreter();
            interpreter.Load(input);
            interpreter.TraceEnabled = trace;
            interpreter.Exec();
            if (disassemble) interpreter.Disassemble();
            if (dump) interpreter.Coredump();
        }

        private static void Test(string fileName)
        {
            using TextReader text_reader = File.OpenText(fileName);

            // Create an input character stream from standard in
            var input = new AntlrInputStream(text_reader);
            // Create an ExprLexer that feeds from that stream
            AssemblerLexer lexer = new AssemblerLexer(input);
            // Create a stream of tokens fed by the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            // Create a parser that feeds off the token stream
            AssemblerParser parser = new AssemblerParser(tokens);

            parser.RemoveErrorListeners(); // remove ConsoleErrorListener
            parser.AddErrorListener(new UnderlineListener()); // add ours
            //parser.AddErrorListener(new DiagnosticErrorListener());

            // Here's how to make the parser report all ambiguities
            //parser.Interpreter.PredictionMode = PredictionMode.SLL;
            parser.Interpreter.PredictionMode = PredictionMode.LL_EXACT_AMBIG_DETECTION;

            // Begin parsing at program rule
            IParseTree tree = parser.program();

            Console.WriteLine(tree.ToStringTree(parser)); // print LISP-style tree
        }
    }
}