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
    public class BacktrackParser : Parser
    {
        public BacktrackParser(Lexer input)
                : base(input)
        {
        }

        /// <summary>
        /// stat : list EOF | assign EOF ;
        /// </summary>
        /// <exception cref="NoViableAltException"></exception>
        public void stat()
        {
            // attempt alternative 1: list EOF
            if (Speculate_stat_alt1())
            {
                list();
                Match(Lexer.EOF_TYPE);
            }
            // attempt alternative 2: assign EOF
            else if (Speculate_stat_alt2())
            {
                assign();
                Match(Lexer.EOF_TYPE);
            }
            // must be an error; neither matched; LT(1) is lookahead token 1
            else throw new NoViableAltException("expecting stat found " + LT(1));
        }

        public bool Speculate_stat_alt1()
        {
            bool success = true;
            Mark(); // mark this spot in input so we can rewind
            try {
                list();
                Match(Lexer.EOF_TYPE); 
            }
            catch (RecognitionException e) 
            {
                success = false; 
            }
            Release(); // either way, rewind to where we were
            return success;
        }

        public bool Speculate_stat_alt2()
        {
            bool success = true;
            Mark(); // mark this spot in input so we can rewind
            try { 
                assign(); 
                Match(Lexer.EOF_TYPE);
            }
            catch (RecognitionException e) 
            { 
                success = false;
            }
            Release(); // either way, rewind to where we were
            return success;
        }

        /// <summary>
        /// assign : list '=' list ; // parallel assignment
        /// </summary>
        public void assign()
        {
            list(); 
            Match(BacktrackLexer.EQUALS_TYPE); 
            list();
        }

        /// <summary>
        /// list : '[' elements ']' ; // match bracketed list
        /// </summary>
        public void list()
        {
            Match(BacktrackLexer.LBRACK_TYPE); 
            elements();
            Match(BacktrackLexer.RBRACK_TYPE);
        }

        /// <summary>
        /// elements : element (',' element)* ; // match comma-separated list
        /// </summary>
        void elements()
        {
            element();
            while (LA(1) == BacktrackLexer.COMMA_TYPE)
            { 
                Match(BacktrackLexer.COMMA_TYPE); 
                element(); 
            }
        }

        /// <summary>
        /// element : name '=' NAME | NAME | list ; // assignment, name or list
        /// </summary>
        /// <exception cref="NoViableAltException"></exception>
        void element()
        {
            if (LA(1) == BacktrackLexer.NAME_TYPE && LA(2) == BacktrackLexer.EQUALS_TYPE)
            {
                Match(BacktrackLexer.NAME_TYPE);
                Match(BacktrackLexer.EQUALS_TYPE);
                Match(BacktrackLexer.NAME_TYPE);
            }
            else if (LA(1) == BacktrackLexer.NAME_TYPE) Match(BacktrackLexer.NAME_TYPE);
            else if (LA(1) == BacktrackLexer.LBRACK_TYPE) list();
            else throw new NoViableAltException("expecting element found " + LT(1));
        }
    }
}


