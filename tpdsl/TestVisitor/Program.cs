/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/

namespace TestVisitor
{
    class Program
    {
        public static IntNode I(int i)
        {
            return new IntNode(new Token(Token.INT, i.ToString()));
        }

        static void Main(string[] args)
        {
            // x = 3+4
            List<StatNode> stats = new List<StatNode>();
            AddNode a = new AddNode(I(3), new Token(Token.PLUS), I(4));
            VarNode x = new VarNode(new Token(Token.ID, "x"));
            AssignNode assign = new AssignNode(x, new Token(Token.ASSIGN, "="), a);
            stats.Add(assign);
            
            // print x * [2, 3, 4]
            Token mult = new Token(Token.MULT, "*");
            List<ExprNode> elements = new List<ExprNode>();
            elements.Add(I(2));
            elements.Add(I(3));
            elements.Add(I(4));
            VectorNode v = new VectorNode(new Token(Token.VEC), elements);
            VarNode xref = new VarNode(new Token(Token.ID, "x"));
            ExprNode pv = new MultNode(xref, mult, v);
            PrintNode p = new PrintNode(new Token(Token.PRINT, "print"), pv);
            stats.Add(p);
            
            StatListNode statlist = new StatListNode(stats);

            // Create visitor and then call visit on root node (statlist)
            PrintVisitor visitor = new PrintVisitor();
            statlist.Visit(visitor); // tell root node to visit with this visitor

            // Create visitor and then visit root node (statlist)
            IndependentPrintVisitor indepVisitor = new IndependentPrintVisitor();
            indepVisitor.Print(statlist); // tell visitor to print from root
        }
    }
}


    
       

