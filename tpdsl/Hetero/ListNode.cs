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
    /// <summary>
    /// A flat tree ==  tree with nil root: (nil child1 child2 ...)
    /// </summary>
    public class ListNode : HeteroAST
    {
        List<HeteroAST> elements = new List<HeteroAST>();

        public ListNode(List<HeteroAST> elements)
        {
            this.elements = elements;
        }
        
        public override string ToStringTree()
        {
            if (elements == null || elements.Count() == 0) return this.ToString();
            
            StringBuilder buf = new StringBuilder();
            
            for (int i = 0; elements != null && i < elements.Count(); i++)
            {
                HeteroAST t = (HeteroAST)elements[i];
                if (i > 0) buf.Append(' ');
                buf.Append(t.ToStringTree());
            }
            
            return buf.ToString();
        }
    }
}

