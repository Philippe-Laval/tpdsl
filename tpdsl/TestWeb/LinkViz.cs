using System;
/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://www.stringtemplate.org/
// https://github.com/antlr/antlrcs

using Antlr4.StringTemplate;

namespace TestWeb
{
    public class Link
    {
        public string From { get; set; }
        public string To { get; set; }

        public Link(String from, String to)
        {
            From = from;
            To = to;
        }
    }

    public class LinkViz
    {
        private TemplateGroup _templates;

        private List<Link> _links = new List<Link>();


        public LinkViz()
        {
            string directory = Directory.GetCurrentDirectory();
            string path = Path.Combine(directory, "DOT.stg");

            _templates = new TemplateGroupFile(path);
        }

        public void AddLink(String from, String to)
        {
            _links.Add(new Link(from, to));
        }

        public override string ToString()
        {
            Template fileST = _templates.GetInstanceOf("file");
            fileST.Add("gname", "testgraph");
            foreach (Link x in _links)
            {
                Template edgeST = _templates.GetInstanceOf("edge");
                edgeST.Add("from", x.From);
                edgeST.Add("to", x.To);
                fileST.Add("edges", edgeST);
            }
            return fileST.Render(); // render (eval) template to text
        }
    }
}
