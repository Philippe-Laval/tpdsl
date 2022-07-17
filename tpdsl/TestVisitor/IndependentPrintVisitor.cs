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
    public class IndependentPrintVisitor
    {
        // visitor dispatch according to node token type
        public void Print(VecMathNode n)
        {
            switch (n.Token?.type)
            {
                case Token.ID:
                    Print((VarNode)n);
                    break;
                case Token.ASSIGN:
                    Print((AssignNode)n);
                    break;
                case Token.PRINT:
                    Print((PrintNode)n);
                    break;
                case Token.PLUS:
                    Print((AddNode)n);
                    break;
                case Token.MULT:
                    Print((MultNode)n);
                    break;
                case Token.DOT:
                    Print((DotProductNode)n);
                    break;
                case Token.INT:
                    Print((IntNode)n);
                    break;
                case Token.VEC:
                    Print((VectorNode)n);
                    break;
                case Token.STAT_LIST:
                    Print((StatListNode)n);
                    break;
                default:
                    // catch unhandled node types
                    throw new Exception("Node " + n.GetType().FullName + " not handled");
            }
        }

        public void Print(StatListNode n)
        {
            foreach (StatNode p in n.elements)
            {
                Print(p);
            }
        }

        public void Print(AssignNode n)
        {
            Print(n.id);           // walk left child
            Console.Write("="); // print operator
            Print(n.value);        // walk right child
            Console.WriteLine();  // print newline
        }

        public void Print(PrintNode n)
        {
            Console.Write("print ");
            Print(n.value);
            Console.WriteLine();
        }

        public void Print(AddNode n)
        {
            Print(n.left);         // walk left child
            Console.Write("+"); // print operator
            Print(n.right);        // walk right child
        }

        public void Print(DotProductNode n)
        {
            Print(n.left);
            Console.Write(".");
            Print(n.right);
        }

        public void Print(MultNode n)
        {
            Print(n.left);
            Console.Write("*");
            Print(n.right);
        }

        public void Print(IntNode n)
        {
            Console.Write(n.ToString());
        }

        public void Print(VarNode n)
        {
            Console.Write(n.ToString());
        }

        public void Print(VectorNode n)
        {
            Console.Write("[");
            if (n.elements != null)
            {
                for (int i = 0; i < n.elements.Count(); i++)
                {
                    ExprNode child = n.elements[i];
                    if (i > 0) Console.Write(", ");
                    Print(child);
                }
            }
            Console.Write("]");
        }
    }
}


