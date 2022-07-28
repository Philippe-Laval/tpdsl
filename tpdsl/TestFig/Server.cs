/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFig
{
    /// <summary>
    /// can refer to Server in a Fig file
    /// </summary>
    public class Server
    {
        /// <summary>
        /// list of references to Sites
        /// </summary>
        public List<Site> Sites { get; set; } = new();  

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("<Server [");
            bool first = true;
            foreach (Site s in Sites)
            {
                if (!first) buf.Append(", ");
                buf.Append("Site " + s.Answers);
                first = false;
            }
            buf.Append("]>");
            return buf.ToString();
        }
    }
}
