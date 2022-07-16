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
    /// Heterogeneous AST node type
    /// </summary>
    public abstract class HeteroAST
    {
        /// <summary>
        /// Node created from which token?
        /// </summary>
        public Token? Token { get; set; }

        /// <summary>
        /// For making nil-rooted nodes
        /// </summary>
        public HeteroAST() 
        {
        }

        public HeteroAST(Token t) 
        {
            Token = t; 
        }

        /// <summary>
        /// Create node from token type; used mainly for imaginary tokens
        /// </summary>
        /// <param name="tokenType"></param>
        public HeteroAST(int tokenType) { this.Token = new Token(tokenType); }

        /// <summary>
        /// Compute string for single node
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        {
            return Token?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Compute string for a whole tree not just node; default: print token
        /// </summary>
        /// <returns></returns>
        public virtual string ToStringTree()
        { 
            return ToString(); 
        }
    }
}

