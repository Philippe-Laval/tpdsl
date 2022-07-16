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
    public class ListParser : Parser
    {
        public ListParser(Lexer input)
                : base(input)
        { }

        /// <summary>
        /// list : '[' elements ']' ; // match bracketed list
        /// </summary>
        public void list()
        {
            Match(ListLexer.LBRACK_TYPE);
            elements();
            Match(ListLexer.RBRACK_TYPE);
        }

        /// <summary>
        /// elements : element (',' element)* ;
        /// </summary>
        void elements()
        {
            element();
            while (lookahead.type == ListLexer.COMMA_TYPE)
            {
                Match(ListLexer.COMMA_TYPE); element();
            }
        }

        /// <summary>
        /// element : name | list ; // element is name or nested list
        /// </summary>
        /// <exception cref="Exception"></exception>
        void element()
        {
            if (lookahead.type == ListLexer.NAME_TYPE) Match(ListLexer.NAME_TYPE);
            else if (lookahead.type == ListLexer.LBRACK_TYPE) list();
            else throw new Exception("expecting name or list; found " + lookahead);
        }
    }
}


