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
    public abstract class Lexer
    {
        public static readonly char EOF = '\0';     //  represent end of file char
        public static readonly int EOF_TYPE = 1;    //  represent EOF token type
        protected string input; // input string
        protected int i = 0;    // index into input of current character
        protected char c;       // current character

        public Lexer(String input)
        {
            this.input = input;
            c = input[i]; // prime lookahead
        }

        /// <summary>
        /// Move to next non-whitespace character
        /// </summary>
        public void Consume()
        { 
            Advance();
            WS();
        }

        /** Move one character; detect "end of file" */
        public void Advance()
        {
            i++;
            if (i >= input.Length)
            {
                c = EOF;
            }
            else
            {
                c = input[i];
            }
        }

        /// <summary>
        /// Ensure x is next character on the input stream
        /// </summary>
        /// <param name="x"></param>
        /// <exception cref="Exception"></exception>
        public void Match(char x)
        {
            if (c == x) Consume();
            else throw new Exception("expecting " + x + "; found " + c);
        }

        public abstract Token NextToken();

        protected abstract void WS();
        
        public abstract String GetTokenName(int x);

    }
}

