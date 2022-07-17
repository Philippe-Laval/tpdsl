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
    public class PrintVisitor : IVecMathVisitor
    {
        public void Visit(AssignNode n)
        {
            n.Id.Visit(this);
            Console.Write("=");
            n.Value.Visit(this);
            Console.WriteLine();
        }

        public void Visit(StatListNode n)
        {
            foreach (StatNode p in n.Elements)
            {
                p.Visit(this);
            }
        }

        public void Visit(PrintNode n)
        {
            Console.Write("print ");
            n.Value.Visit(this);
            Console.WriteLine();
        }

        public void Visit(AddNode n)
        {
            n.Left.Visit(this);
            Console.Write("+");
            n.Right.Visit(this);
        }

        public void Visit(DotProductNode n)
        {
            n.Left.Visit(this);
            Console.Write(".");
            n.Right.Visit(this);
        }

        public void Visit(MultNode n)
        {
            n.Left.Visit(this);
            Console.Write("*");
            n.Right.Visit(this);
        }

        public void Visit(IntNode n)
        {
            Console.Write(n.ToString());
        }

        public void Visit(VarNode n)
        {
            Console.Write(n.ToString());
        }

        public void Visit(VectorNode n)
        {
            Console.Write("[");
            if (n.Elements != null)
            {
                for (int i = 0; i < n.Elements.Count(); i++)
                {
                    ExprNode child = n.Elements[i];
                    if (i > 0) Console.Write(", ");
                    child.Visit(this);
                }
            }
            Console.Write("]");
        }
    }
}

