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

namespace Normalized
{
    public class AddNode : ExprNode
    {
        public AddNode(ExprNode left, Token addToken, ExprNode right)
                : base(addToken)
        {
            AddChild(left);
            AddChild(right);
        }

        public override int GetEvalType()
        {
            if (Children is not null)
            {
                ExprNode left = (ExprNode)Children[0];
                ExprNode right = (ExprNode)Children[1];

                if (left.GetEvalType() == tINTEGER && right.GetEvalType() == tINTEGER)
                {
                    return tINTEGER;
                }

                if (left.GetEvalType() == tVECTOR && right.GetEvalType() == tVECTOR)
                {
                    return tVECTOR;
                }
            }

            return tINVALID;
        }
    }

}

