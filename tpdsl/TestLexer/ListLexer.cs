using System;
/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLexer
{
    public class ListLexer : Lexer
    {
        public static readonly int NAME_TYPE = 2;
        public static readonly int COMMA_TYPE = 3;
        public static readonly int LBRACK_TYPE = 4;
        public static readonly int RBRACK_TYPE = 5;
        public static readonly string[] tokenNames = new string[] { "n/a", "<EOF>", "NAME", "COMMA", "LBRACK", "RBRACK" };

        public override String GetTokenName(int x) { return tokenNames[x]; }

        public ListLexer(string input)
                : base(input)
        { }

        bool IsLETTER() { return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z'; }

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
                        return new Token(COMMA_TYPE, ",");
                    case '[':
                        Consume();
                        return new Token(LBRACK_TYPE, "[");
                    case ']':
                        Consume();
                        return new Token(RBRACK_TYPE, "]");
                    default:
                        if (IsLETTER()) return NAME();
                        throw new Exception("invalid character: " + c);
                }
            }

            return new Token(EOF_TYPE, "<EOF>");
        }

        /// <summary>
        /// NAME : ('a'..'z'|'A'..'Z')+; // NAME is sequence of >=1 letter
        /// </summary>
        /// <returns></returns>
        Token NAME()
        {
            StringBuilder buf = new StringBuilder();
            do
            {
                buf.Append(c);
                Consume();
            } while (IsLETTER());

            return new Token(NAME_TYPE, buf.ToString());
        }

        /// <summary>
        /// WS : (' '|'\t'|'\n'|'\r')* ; // ignore any whitespace
        /// </summary>
        void WS()
        {
            while (c == ' ' || c == '\t' || c == '\n' || c == '\r')
            {
                Consume();
            }
        }
    }
}