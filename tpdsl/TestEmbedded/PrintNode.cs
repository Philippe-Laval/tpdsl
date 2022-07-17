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

namespace TestEmbedded
{
    public class PrintNode : StatNode
    {
        public ExprNode Value { get; set; }

        public PrintNode(Token t, ExprNode value)
                : base(t)
        {
            this.Value = value;
        }

        public override void Print()
        {
            Console.Write("print ");
            Value.Print();
            Console.WriteLine();
        }
    }
}


