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

namespace TestMulti
{
    public class Token
    {
        public int type;
        public String text;

        public Token(int type, String text) { this.type = type; this.text = text; }
        
        public override string ToString()
        {
            String tname = LookaheadLexer.tokenNames[type];
            return "<'" + text + "'," + tname + ">";
        }
    }
}