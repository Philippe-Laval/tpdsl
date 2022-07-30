/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using Antlr4.StringTemplate;

namespace TestAst
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree t = new Tree("VAR");
            t.AddChild(new Tree("int"));
            t.AddChild(new Tree("x"));

            Tree m = new Tree("+");
            m.AddChild(new Tree("3"));
            m.AddChild(new Tree("4"));

            t.AddChild(m);
            
            ASTViz viz = new ASTViz(t);
            Console.WriteLine(viz.ToString());
        }
    }
}