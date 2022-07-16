/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/

using Normalized;

Token plus = new Token(Token.PLUS, "+");
Token oneToken = new Token(Token.INT, "1");
Token twoToken = new Token(Token.INT, "2");
ExprNode root = new AddNode(new IntNode(oneToken), plus, new IntNode(twoToken));
Console.WriteLine(root.ToStringTree());
Console.WriteLine($"Eval type : {root.GetEvalType()}");

Token threeToken = new Token(Token.INT, "3");
Token foorToken = new Token(Token.INT, "4");
IntNode oneIntNode = new IntNode(oneToken);
IntNode twoIntNode = new IntNode(twoToken);
IntNode threeIntNode = new IntNode(threeToken);
IntNode foorIntNode = new IntNode(foorToken);

Token vectToken = new Token(Token.VECT, "VECT");
VectorNode vectorNode1 = new VectorNode(vectToken, new List<ExprNode> { oneIntNode, twoIntNode });
Console.WriteLine(vectorNode1.ToStringTree());
Console.WriteLine($"Eval type : {vectorNode1.GetEvalType()}");

VectorNode vectorNode2 = new VectorNode(vectToken, new List<ExprNode> { threeIntNode, foorIntNode });
Console.WriteLine(vectorNode1.ToStringTree());
Console.WriteLine($"Eval type : {vectorNode1.GetEvalType()}");

ExprNode root2 = new AddNode(vectorNode1, plus, vectorNode2);
Console.WriteLine(root2.ToStringTree());
Console.WriteLine($"Eval type : {root2.GetEvalType()}");

