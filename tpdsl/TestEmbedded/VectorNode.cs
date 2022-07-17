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
    public class VectorNode : ExprNode
    {
        public List<ExprNode> Elements { get; set; } = new List<ExprNode>();

        public VectorNode(Token t, List<ExprNode> elements)
                : base(t) // track vector token; most likely it's an imaginary token
        {
            this.Elements = elements;
        }

        public override void Print()
        {
            Console.Write("[");
            if (Elements != null)
            {
                for (int i = 0; i < Elements.Count(); i++)
                {
                    ExprNode child = Elements[i];
                    if (i > 0) Console.Write(", ");
                    child.Print();
                }
            }
            Console.Write("]");
        }
    }
}