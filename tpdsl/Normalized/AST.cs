using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using System.Threading.Tasks;

namespace Normalized
{
    /// <summary>
    /// Homogeneous AST node type
    /// </summary>
    public class AST
    {
        /// <summary>
        /// From which token did we create node?
        /// </summary>
        public Token? Token { get; set; }
        /// <summary>
        /// normalized list of children
        /// </summary>
        public List<AST>? Children { get; set; }

        /// <summary>
        /// for making nil-rooted nodes
        /// </summary>
        public AST() 
        { 
        } 
        
        public AST(Token t) 
        {
            Token = t; 
        }

        /// <summary>
        /// Create node from token type; used mainly for imaginary tokens 
        /// </summary>
        /// <param name="tokenType"></param>
        public AST(int tokenType) { this.Token = new Token(tokenType); }

        /// <summary>
        /// External visitors need unique int per node for id purposes while walking.
        /// </summary>
        /// <returns></returns>
        public int GetNodeType() { return Token?.Type ?? Token.INVALID_TOKEN_TYPE; }

        /// <summary>
        /// Add as kid
        /// </summary>
        /// <param name="t"></param>
        public void AddChild(AST t)
        {
            if (Children == null) Children = new List<AST>();
            Children.Add(t);
        }

        public bool IsNil() { return Token == null; }

        /// <summary>
        /// Compute string for single node
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        { 
            return Token?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Compute string for a whole tree not just a node
        /// </summary>
        /// <returns></returns>
        public string ToStringTree()
        {
            if (Children == null || Children.Count() == 0) return this.ToString();

            StringBuilder buf = new StringBuilder();
            
            if (!IsNil())
            {
                buf.Append("(");
                buf.Append(this.ToString());
                buf.Append(' ');
            }

            for (int i = 0; Children != null && i < Children.Count(); i++)
            {
                AST t = (AST)Children[i]; // normalized (unnamed) children
                if (i > 0) buf.Append(' ');
                buf.Append(t.ToStringTree());
            }

            if (!IsNil()) buf.Append(")");
            
            return buf.ToString();
        }
    }
}

