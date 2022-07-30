/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using Antlr4.StringTemplate;

namespace TestWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            string assign = "<left> = <right>;";
            Template st = new Template(assign, '<', '>');
            st.Add("left", "x");   // attribute left is a string
            st.Add("right", 99);   // attribute right is an integer
            string output = st.Render();  // render template to text
            Console.WriteLine(output);
        }
    }
}
