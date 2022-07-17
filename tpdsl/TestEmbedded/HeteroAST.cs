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
    /// <summary>
    /// Heterogeneous AST node type
    /// </summary>
    public class HeteroAST
    {
        /// <summary>
        /// This node created from which token?
        /// </summary>
        public Token? Token { get; set; } = null; 
        
        public HeteroAST() 
        {
        }

        public HeteroAST(Token t)
        { 
            this.Token = t; 
        }

        public override string ToString()
        {
            return Token?.ToString() ?? string.Empty; 
        }
    }
}