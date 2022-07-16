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

namespace TestBacktrack
{
    public abstract class Parser
    {
        Lexer input;           // from where do we get tokens?
        List<int> markers;     // stack of index markers into lookahead buffer
        List<Token> lookahead; // dynamically-sized lookahead buffer
        int p = 0;             // index of current lookahead token;
                               // LT(1) returns lookahead[p]

        public Parser(Lexer input)
        {
            this.input = input;
            markers = new List<int>(); // make marker stack
            lookahead = new List<Token>(); // make lookahead buffer
            Sync(1); // prime buffer with at least 1 token
        }

        public void Consume()
        {
            p++;
            // have we hit end of buffer when not backtracking?
            if (p == lookahead.Count() && !IsSpeculating())
            {
                // if so, it's an opportunity to start filling at index 0 again
                p = 0;
                lookahead.Clear(); // size goes to 0, but retains memory
            }
            Sync(1); // get another to replace consumed token
        }

        /** Make sure we have i tokens from current position p */
        public void Sync(int i)
        {
            if (p + i - 1 > (lookahead.Count() - 1))
            {       // out of tokens?
                int n = (p + i - 1) - (lookahead.Count() - 1); // get n tokens
                Fill(n);
            }
        }

        public void Fill(int n)
        {
            // add n tokens
            for (int i = 1; i <= n; i++)
            {
                lookahead.Add(input.NextToken());
            }
        }

        public Token LT(int i)
        {
            Sync(i);
            return lookahead[p + i - 1];
        }

        public int LA(int i)
        {
            return LT(i).type;
        }

        public void Match(int x)
        {
            if (LA(1) == x) Consume();
            else throw new MismatchedTokenException("expecting " + input.GetTokenName(x) + " found " + LT(1));
        }

        public int Mark()
        {
            markers.Add(p);
            return p;
        }

        public void Release()
        {
            int marker = markers[markers.Count() - 1];
            markers.Remove(markers.Count() - 1);
            Seek(marker);
        }

        public void Seek(int index)
        {
            p = index;
        }

        public bool IsSpeculating()
        {
            return markers.Count() > 0;
        }
    }
}