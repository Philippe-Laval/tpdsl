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
    public class ListNode : ExprNode
    {
        public ListNode(Token t, List<ExprNode> elements)
                : base(t) // track vector token; most likely it's an imaginary token
        {
            _evalType = tVECTOR;

            foreach (ExprNode e in elements)
            {
                // all elements as kids
                AddChild(e);
            }
        }
    }
}