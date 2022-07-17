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
    /// <summary>
    /// A generic heterogeneous tree node used in our vector math trees
    /// </summary>
    public abstract class VecMathNode : HeteroAST
    {
        public VecMathNode() 
            : base()
        {
        }

        public VecMathNode(Token t)
            : base(t)
        {
        }

        /// <summary>
        /// dispatcher 
        /// </summary>
        /// <param name="visitor"></param>
        public abstract void Visit(IVecMathVisitor visitor);
    }
}

