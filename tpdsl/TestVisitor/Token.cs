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

namespace TestVisitor
{
    public class Token
    {
        public const int INVALID_TOKEN_TYPE = 0;
        public const int PLUS = 1; // token types
        public const int MULT = 2;
        public const int DOT = 3;
        public const int INT = 4;
        public const int VEC = 5;
        public const int ID = 6;
        public const int ASSIGN = 7;
        public const int PRINT = 8;
        public const int STAT_LIST = 9;

        public int type = INVALID_TOKEN_TYPE;
        public string text = string.Empty;

        public Token(int type, string text)
        {
            this.type = type; 
            this.text = text; 
        }

        public Token(int type)
        { 
            this.type = type; 
        }

        public override string ToString() 
        { 
            return text;
        }
    }
}


