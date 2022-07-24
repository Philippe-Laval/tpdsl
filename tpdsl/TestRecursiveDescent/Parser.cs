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

namespace TestRecursiveDescent
{
    public abstract class Parser
    {
        Lexer input;     // from where do we get tokens?
        protected Token lookahead = null!; // the current lookahead token

        public Parser(Lexer input) 
        {
            this.input = input;
            Consume(); 
        }

        /// <summary>
        /// If lookahead token type matches x, consume & return else error
        /// </summary>
        /// <param name="x"></param>
        /// <exception cref="Exception"></exception>
        public void Match(int x)
        {
            if (lookahead.type == x) Consume();
            else throw new Exception("expecting " + input.GetTokenName(x) +
                                 "; found " + lookahead);
        }

        public void Consume()
        {
            lookahead = input.NextToken();
        }
    }
}