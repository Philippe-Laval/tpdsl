using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;

namespace TestTree
{
    class Program
    {
        static void Main(string[] args)
        {
            bool checkStatic = true;


            if (checkStatic)
            {
                Console.WriteLine("Checking syntax");

                Console.WriteLine("test.pie");
                Test1("test.pie");

                Console.WriteLine("apple.pie");
                Test1("apple.pie");

                Console.WriteLine("cherry.pie");      
                Test1("cherry.pie");

                Console.WriteLine("factorial.pie");
                Test1("factorial.pie");

                Console.WriteLine("forward.pie");
                Test1("forward.pie");

                Console.WriteLine("localstruct.pie");         
                Test1("localstruct.pie");
                
                Console.WriteLine("lookup.pie");   
                Test1("lookup.pie");

                Console.WriteLine("loop.pie"); 
                Test1("loop.pie");

                Console.WriteLine("struct.pie"); 
                Test1("struct.pie");

                Console.WriteLine("structerrors.pie"); 
                Test1("structerrors.pie");
            }

            Console.WriteLine("Interpreter");

            Console.WriteLine("test.pie");
            Test2("test.pie");
            Console.WriteLine("apple.pie");
            Test2("apple.pie");
            Console.WriteLine("cherry.pie");
            Test2("cherry.pie");
            Console.WriteLine("factorial.pie");
            Test2("factorial.pie");
            Console.WriteLine("forward.pie");
            Test2("forward.pie");
            Console.WriteLine("lookup.pie");
            Test2("lookup.pie");
            Console.WriteLine("localstruct.pie");
            Test2("localstruct.pie");
            Console.WriteLine("loop.pie");
            Test2("loop.pie");
            Console.WriteLine("struct.pie");
            Test2("struct.pie");
            Console.WriteLine("structerrors.pie");
            Test2("structerrors.pie");
        }

        private static void Test2(string fileName)
        {
            using TextReader input = File.OpenText(fileName);

            Interpreter interp = new Interpreter();
            interp.Interp(input);
        }

        private static void Test1(string fileName)
        {
            using TextReader text_reader = File.OpenText(fileName);

            // Create an input character stream from standard in
            var input = new AntlrInputStream(text_reader);
            // Create an ExprLexer that feeds from that stream
            PieLexer lexer = new PieLexer(input);
            // Create a stream of tokens fed by the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);

            Interpreter interp = new Interpreter();

            // Create a parser that feeds off the token stream
            PieParser parser = new PieParser(tokens, interp);

            parser.RemoveErrorListeners(); // remove ConsoleErrorListener
            parser.AddErrorListener(new UnderlineListener()); // add ours
            //parser.AddErrorListener(new DiagnosticErrorListener());

            // Here's how to make the parser report all ambiguities
            //parser.Interpreter.PredictionMode = PredictionMode.SLL;
            parser.Interpreter.PredictionMode = PredictionMode.LL;

            // Begin parsing at file rule
            IParseTree tree = parser.program();

            Console.WriteLine(tree.ToStringTree(parser)); // print LISP-style tree
        }
    }
}