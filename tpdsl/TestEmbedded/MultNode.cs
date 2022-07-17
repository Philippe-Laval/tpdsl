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
    public class MultNode : ExprNode
    {
        public ExprNode Left { get; set; } // named, node-specific, irregular children
        public ExprNode Right { get; set; }

        public MultNode(ExprNode left, Token t, ExprNode right)
                : base(t)
        {
            this.Left = left;
            this.Right = right;
        }

        public override void Print()
        {
            Left.Print();
            Console.Write("*");
            Right.Print();
        }
    }
}


