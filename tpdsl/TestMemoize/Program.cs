﻿/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/

namespace TestMemoize
{
    class Program
    {
        static void Main(string[] args)
        {
            Test("input.txt");
        }

        private static void Test(string fileName)
        {
            using TextReader text_reader = File.OpenText(fileName);
            string input = text_reader.ReadToEnd();

            BacktrackLexer lexer = new BacktrackLexer(input); 
            BacktrackParser parser = new BacktrackParser(lexer);
            parser.stat(); // begin parsing at rule stat
        }
    }
}