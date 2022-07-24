using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;

namespace TestAggr
{
    class Program
    {
        public static void Error(IToken t, string msg)
        {
            Console.WriteLine($"line {t.Line}:{t.Column} {msg}");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("t.cymbol");

            Test("t.cymbol");

            Console.WriteLine("t2.cymbol");

            Test("t2.cymbol");
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
            parser.BuildParseTree = true;

            parser.RemoveErrorListeners(); // remove ConsoleErrorListener
            parser.AddErrorListener(new UnderlineListener()); // add ours
            //parser.AddErrorListener(new DiagnosticErrorListener());

            // Here's how to make the parser report all ambiguities
            //parser.Interpreter.PredictionMode = PredictionMode.SLL;
            parser.Interpreter.PredictionMode = PredictionMode.LL_EXACT_AMBIG_DETECTION;

            // Begin parsing at file rule
            IParseTree tree = parser.compilationUnit();

            Console.WriteLine(tree.ToStringTree(parser)); // print LISP-style tree

            ParseTreeWalker walker = new ParseTreeWalker();

            SymbolTable symtab = new SymbolTable(); // make global scope, types

            DefPhase defPhase = new DefPhase(symtab);
            walker.Walk(defPhase, tree);

            // create next phase and feed symbol table info from def to ref phase
            RefPhase refPhase = new RefPhase(symtab, defPhase.scopes);
            walker.Walk(refPhase, tree);
        }
    }
}