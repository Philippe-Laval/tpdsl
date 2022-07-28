using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;
using static TestFig.FigParser;

namespace TestFig
{
    class Program
    {


        static void Main(string[] args)
        {
            using TextReader text_reader = File.OpenText("jguru.fig");

            Dictionary<string, object> config_objects = ReadFig(text_reader);
            foreach (var kvp in config_objects)
            {
                Console.WriteLine(kvp.Key + ":" + kvp.Value);
            }
        }

        public static Dictionary<string, object> ReadFig(TextReader text_reader)
        {
            // Create an input character stream from standard in
            var input = new AntlrInputStream(text_reader);
            FigLexer lexer = new FigLexer(input);
            // Create a buffer of tokens feeding off of lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            // Create parser feeding off of token buffer
            MyFigParser p = new MyFigParser(tokens);
            FileContext fileContext = p.file(); // parse, return dictionary of config'd objects
            return fileContext.Instances;
        }
    }
}