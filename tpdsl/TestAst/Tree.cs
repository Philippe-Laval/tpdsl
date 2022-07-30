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

namespace TestAst
{
    public class Tree
    {
        /// <summary>
        /// what each node holds
        /// </summary>
        public string Payload { get; set; }
        /// <summary>
        /// any children
        /// </summary>
        public List<Tree> Children { get; set; } = new List<Tree>(); 

        public Tree(string payload)
        {
            this.Payload = payload;
        }

        public void AddChild(Tree t) 
        {
            Children.Add(t); 
        }

        public int GetChildCount()
        { 
            return Children.Count(); 
        }
    }
}
