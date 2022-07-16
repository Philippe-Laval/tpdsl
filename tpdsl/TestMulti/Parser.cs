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

namespace TestMulti
{
    public abstract class Parser
    {
        Lexer input;       // from where do we get tokens?
        Token[] lookahead; // circular lookahead buffer
        int k;             // how many lookahead symbols
        int p = 0;         // circular index of next token position to fill

        public Parser(Lexer input, int k)
        {
            this.input = input;
            this.k = k;
            lookahead = new Token[k];           // make lookahead buffer
            for (int i = 1; i <= k; i++) Consume(); // prime buffer with k lookahead
        }

        public void Consume()
        {
            lookahead[p] = input.NextToken();     // fill next position with token
            p = (p + 1) % k;                      // increment circular index
        }

        public Token LT(int i) 
        {
            // circular fetch
            return lookahead[(p + i - 1) % k];
        }
       
        public int LA(int i) 
        {
            return LT(i).type;
        }

        public void Match(int x)
        {
            if (LA(1) == x) Consume();
            else throw new Exception("expecting " + input.GetTokenName(x) + "; found " + LT(1));
        }
    }
}