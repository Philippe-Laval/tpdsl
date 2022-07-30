/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
namespace TestWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkViz viz = new LinkViz();
            viz.AddLink("index.html", "login.html");
            viz.AddLink("index.html", "about.html");
            viz.AddLink("login.html", "error.html");
            viz.AddLink("about.html", "news.html");
            viz.AddLink("index.html", "news.html");
            viz.AddLink("logout.html", "index.html");
            viz.AddLink("index.html", "logout.html");
            Console.WriteLine(viz.ToString());
        }
    }
}

