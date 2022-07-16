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
    public class LookaheadLexer : Lexer
    {
        public static int NAME = 2;
        public static int COMMA = 3;
        public static int LBRACK = 4;
        public static int RBRACK = 5;
        public static int EQUALS = 6;
        public static string[] tokenNames = new string[] { "n/a", "<EOF>", "NAME", ",", "[", "]", "=" };
        
        public override string GetTokenName(int x)
        {
            return LookaheadLexer.tokenNames[x]; 
        }

        public LookaheadLexer(string input)
            :base (input)
        {  }
        
        bool IsLETTER()
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z'; 
        }

        public override Token NextToken()
        {
            while (c != EOF)
            {
                switch (c)
                {
                    case ' ':
                    case '\t':
                    case '\n':
                    case '\r': 
                        WS(); 
                        continue;
                    case ',': 
                        Consume(); 
                        return new Token(COMMA, ",");
                    case '[':
                        Consume();
                        return new Token(LBRACK, "[");
                    case ']':
                        Consume(); 
                        return new Token(RBRACK, "]");
                    case '=':
                        Consume(); 
                        return new Token(EQUALS, "=");
                    default:
                        if (IsLETTER()) return name();
                        throw new Exception("invalid character: " + c);
                }
            }
            return new Token(EOF_TYPE, "<EOF>");
        }

        /// <summary>
        /// name : LETTER+ ; // name is sequence of >=1 letter
        /// </summary>
        /// <returns></returns>
        Token name()
        {
            StringBuilder buf = new StringBuilder();
            
            do { 
                buf.Append(c);
                LETTER(); 
            } while (IsLETTER());

            return new Token(NAME, buf.ToString());
        }

        /// <summary>
        /// LETTER   : 'a'..'z'|'A'..'Z'; // define what a letter is (\w)
        /// </summary>
        /// <exception cref="Error"></exception>
        void LETTER()
        {
            if (IsLETTER()) Consume();
            else throw new Exception("expecting LETTER; found " + c);
        }

        /// <summary>
        /// WS : (' '|'\t'|'\n'|'\r')* ; // ignore any whitespace
        /// </summary>
        override protected void WS()
        {
            while (c == ' ' || c == '\t' || c == '\n' || c == '\r') Advance();
        }
    }
}

