/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/

using Homo;

Token plus = new Token(Token.PLUS, "+");
Token one = new Token(Token.INT, "1");
Token two = new Token(Token.INT, "2");
AST root = new AST(plus);
root.AddChild(new AST(one));
root.AddChild(new AST(two));
Console.WriteLine("1+2 tree: " + root.ToStringTree());

AST list = new AST(); // make nil node as root for a list
list.AddChild(new AST(one));
list.AddChild(new AST(two));
Console.WriteLine("1 and 2 in list: " + list.ToStringTree());
  