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
    public class AssignNode : StatNode
    {
        public VarNode Id { get; set; }
        public ExprNode Value { get; set; }

        public AssignNode(VarNode id, Token t, ExprNode value)
            : base(t)
        {
            this.Id = id;
            this.Value = value;
        }

        public override void Print()
        {
            Id.Print();            // walk left child
            Console.Write("="); // print operator
            Value.Print();         // walk right child
            Console.WriteLine();
        }
    }
}