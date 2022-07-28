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
    public class Site
    { 
        // can refer to Site in a Fig file
        public int Port { get; set; }
        public string Answers { get; protected set; } = string.Empty; // set with setAnswers
        public List<string> Aliases { get; set; } = new();

        /// <summary>
        /// The reflection support code looks for setters first then fields
        /// </summary>
        /// <param name="answers"></param>
        public void SetAnswers(string answers)
        {
            Answers = answers; 
        }

        public override string ToString()
        {
            return $"<Site {Answers}:{Port}" + 
                   (Aliases == null ? "" : $",aliases={string.Join(", ", Aliases)}") + ">";
        }
    }
}


