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

namespace TestVisitor
{
    public class DotProductNode : ExprNode
    {
        public ExprNode left { get; set; } // named, node-specific, irregular children
        public ExprNode right { get; set; }

        public DotProductNode(ExprNode left, Token t, ExprNode right)
            : base(t)
        {
            this.left = left;
            this.right = right;
        }

        public override void Visit(IVecMathVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}