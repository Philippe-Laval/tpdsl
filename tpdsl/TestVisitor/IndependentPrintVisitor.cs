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
        public void print(VecMathNode n)
        {
            switch (n.Token?.type)
            {
                case Token.ID:
                    print((VarNode)n);
                    break;
                case Token.ASSIGN:
                    print((AssignNode)n);
                    break;
                case Token.PRINT:
                    print((PrintNode)n);
                    break;
                case Token.PLUS:
                    print((AddNode)n);
                    break;
                case Token.MULT:
                    print((MultNode)n);
                    break;
                case Token.DOT:
                    print((DotProductNode)n);
                    break;
                case Token.INT:
                    print((IntNode)n);
                    break;
                case Token.VEC:
                    print((VectorNode)n);
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
                print(p);
            }
        }

        public void print(AssignNode n)
        {
            print(n.id);           // walk left child
            Console.Write("="); // print operator
            print(n.value);        // walk right child
            Console.WriteLine();  // print newline
        }

        public void print(PrintNode n)
        {
            Console.Write("print ");
            print(n.value);
            Console.WriteLine();
        }

        public void print(AddNode n)
        {
            print(n.left);         // walk left child
            Console.Write("+"); // print operator
            print(n.right);        // walk right child
        }

        public void print(DotProductNode n)
        {
            print(n.left);
            Console.Write(".");
            print(n.right);
        }

        public void print(MultNode n)
        {
            print(n.left);
            Console.Write("*");
            print(n.right);
        }

        public void print(IntNode n)
        {
            Console.Write(n.ToString());
        }

        public void print(VarNode n)
        {
            Console.Write(n.ToString());
        }

        public void print(VectorNode n)
        {
            Console.Write("[");
            if (n.elements != null)
            {
                for (int i = 0; i < n.elements.Count(); i++)
                {
                    ExprNode child = n.elements[i];
                    if (i > 0) Console.Write(", ");
                    print(child);
                }
            }
            Console.Write("]");
        }
    }
}


