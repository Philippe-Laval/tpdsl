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
    public abstract class ExprNode : AST
    {
        /// <summary>
        /// invalid expression type
        /// </summary>
        public static readonly int tINVALID = 0;
        /// <summary>
        /// integer expression type
        /// </summary>
        public static readonly int tINTEGER = 1;
        /// <summary>
        /// vector expression type
        /// </summary>
        public static readonly int tVECTOR = 2;

        /// <summary>
        /// Track expression type (integer or vector) for each expr node.
        ///  This is the type of the associated value not the getNodeType()
        ///  used by an external visitor to distinguish between nodes.
        /// </summary>
        protected int _evalType;

        public virtual int GetEvalType() { return _evalType; }

        public ExprNode(Token payload)
                : base(payload)
        { }

        /// <summary>
        /// ExprNode's know about the type of an expresson, include that
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_evalType != tINVALID)
            {
                return base.ToString() + "<type=" + (_evalType == tINTEGER ? "tINTEGER" : "tVECTOR") + ">";
            }
            return base.ToString();
        }

    }
}


