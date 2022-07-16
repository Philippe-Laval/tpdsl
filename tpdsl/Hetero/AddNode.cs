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

namespace Hetero
{
    public class AddNode : ExprNode
    {
        // named, node-specific, irregular children
        public ExprNode Left { get; set; }
        public ExprNode Right { get; set; }

        public AddNode(ExprNode left, Token addToken, ExprNode right)
             : base(addToken)
        {
            this.Left = left;
            this.Right = right;
        }

        public override string ToStringTree()
        {
            if (Left == null || Right == null) return this.ToString();

            StringBuilder buf = new StringBuilder();
            buf.Append("(");
            buf.Append(this.ToString());
            buf.Append(' ');
            buf.Append(Left.ToStringTree());
            buf.Append(' ');
            buf.Append(Right.ToStringTree());
            buf.Append(")");

            return buf.ToString();
        }
    }
}

