﻿/***
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

namespace TestLexer
{
    public class Token
    {
        public int Type { get; set; }
        public string Text { get; set; }

        public Token(int type, string text)
        {
            this.Type = type;
            this.Text = text;
        }

        public override string ToString()
        {
            string tname = ListLexer.tokenNames[Type];
            return "<'" + Text + "'," + tname + ">";
        }
    }
}