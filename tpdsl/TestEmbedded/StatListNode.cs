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

namespace TestEmbedded
{
    /// <summary>
    /// A flat tree ==  tree with nil root: (nil child1 child2 ...)
    /// </summary>
    public class StatListNode : VecMathNode
    {
        public List<StatNode> Elements { get; set; } = new List<StatNode>();

        public StatListNode(List<StatNode> elements)
            : base(new Token(Token.STAT_LIST)) // create imaginary token
        {
            this.Elements = elements;
        }

        public override void Print()
        {
            foreach (VecMathNode n in Elements) n.Print();
        }

    }
}



