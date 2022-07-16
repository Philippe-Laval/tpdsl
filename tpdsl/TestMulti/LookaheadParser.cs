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
    public class LookaheadParser : Parser
    {
        public LookaheadParser(Lexer input, int k)
            : base(input, k)
        { }

        /// <summary>
        /// list : '[' elements ']' ; // match bracketed list
        /// </summary>
        public void list()
        {
            Match(LookaheadLexer.LBRACK); 
            elements();
            Match(LookaheadLexer.RBRACK);
        }

        /// <summary>
        /// elements : element (',' element)* ; // match comma-separated list
        /// </summary>
        void elements()
        {
            element();
            while (LA(1) == LookaheadLexer.COMMA)
            { 
                Match(LookaheadLexer.COMMA);
                element(); 
            }
        }

        /** element : NAME '=' NAME | NAME | list ; assignment, NAME or list */
        void element()
        {
            if (LA(1) == LookaheadLexer.NAME && LA(2) == LookaheadLexer.EQUALS)
            {
                Match(LookaheadLexer.NAME);
                Match(LookaheadLexer.EQUALS);
                Match(LookaheadLexer.NAME);
            }
            else if (LA(1) == LookaheadLexer.NAME) Match(LookaheadLexer.NAME);
            else if (LA(1) == LookaheadLexer.LBRACK) list();
            else throw new Exception("expecting name or list; found " + LT(1));
        }
    }
}
