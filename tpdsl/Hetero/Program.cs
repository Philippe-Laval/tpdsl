/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/

using Hetero;

Token plusToken = new Token(Token.PLUS, "+");
Token oneToken = new Token(Token.INT, "1");
Token twoToken = new Token(Token.INT, "2");
ExprNode root = new AddNode(new IntNode(oneToken), plusToken, new IntNode(twoToken));
Console.WriteLine(root.ToStringTree());

Token threeToken = new Token(Token.INT, "3");
Token foorToken = new Token(Token.INT, "4");
IntNode oneIntNode = new IntNode(oneToken);
IntNode twoIntNode = new IntNode(twoToken);
IntNode threeIntNode = new IntNode(threeToken);
IntNode foorIntNode = new IntNode(foorToken);

Token vectToken = new Token(Token.VECT, "VECT");
VectorNode vectorNode1 = new VectorNode(vectToken, new List<ExprNode> { oneIntNode, twoIntNode });
Console.WriteLine(vectorNode1.ToStringTree());

VectorNode vectorNode2 = new VectorNode(vectToken, new List<ExprNode> { threeIntNode, foorIntNode });
Console.WriteLine(vectorNode1.ToStringTree());

ExprNode root2 = new AddNode(vectorNode1, plusToken, vectorNode2);
Console.WriteLine(root2.ToStringTree());