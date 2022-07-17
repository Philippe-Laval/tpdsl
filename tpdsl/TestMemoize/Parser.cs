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

namespace TestMemoize
{
    public abstract class Parser
    {
        public static readonly int FAILED = -1;  // parsing failed on last attempt

        Lexer input;           // from where do we get tokens?
        List<int> markers;     // stack of index markers into lookahead buffer
        List<Token> lookahead; // dynamically-sized lookahead buffer
        int p = 0;             // index of current lookahead token;
                               // LT(1) returns lookahead[p]

        public Parser(Lexer input)
        {
            this.input = input;
            markers = new List<int>(); // make marker stack
            lookahead = new List<Token>(); // make lookahead buffer
            Sync(1); // prime buffer with at least 1 token
        }

        public void Consume()
        {
            p++;
            // have we hit end of buffer when not backtracking?
            if (p == lookahead.Count() && !IsSpeculating())
            {
                // if so, it's an opportunity to start filling at index 0 again
                p = 0;
                lookahead.Clear(); // size goes to 0, but retains memory
                ClearMemo();       // clear any rule_memo dictionaries
            }
            Sync(1); // get another to replace consumed token
        }

        /// <summary>
        /// Make sure we have i tokens from current position p
        /// </summary>
        /// <param name="i"></param>
        public void Sync(int i)
        {
            if (p + i - 1 > (lookahead.Count() - 1))
            {       // out of tokens?
                int n = (p + i - 1) - (lookahead.Count() - 1); // get n tokens
                Fill(n);
            }
        }

        public void Fill(int n)
        { // add n tokens
            for (int i = 1; i <= n; i++) { lookahead.Add(input.NextToken()); }
        }

        public Token LT(int i) { 
            Sync(i); 
            return lookahead[p + i - 1]; 
        }
        
        public int LA(int i)
        {
            return LT(i).type; 
        }
        
        public void Match(int x)
        {
            if (LA(1) == x) Consume();
            else throw new MismatchedTokenException("expecting " +
                           input.GetTokenName(x) + " found " + LT(1));
        }

        /// <summary>
        /// Actual parser implements to clear any rule_memo dictionaries
        /// </summary>
        public abstract void ClearMemo();

        public int Mark()
        {
            markers.Add(p);
            return p;
        }

        public void Release()
        {
            int marker = markers[markers.Count() - 1];
            markers.Remove(markers.Count() - 1);
            Seek(marker);
        }

        public void Seek(int index)
        {
            p = index;
        }

        public bool IsSpeculating()
        {
            return markers.Count() > 0;
        }

        /// <summary>
        /// Have we parsed a particular rule before at this input position?
        ///  If no memoization value, we've never parsed here before.
        ///  If memoization value is FAILED, we parsed and failed before.
        /// If value >= 0, it is an index into the token buffer.It indicates
        /// a previous successful parse.This method has a side effect:
        ///  it seeks ahead in the token buffer to avoid reparsing.
        /// </summary>
        /// <param name="memoization"></param>
        /// <returns></returns>
        /// <exception cref="PreviousParseFailedException"></exception>
        public bool AlreadyParsedRule(Dictionary<int, int> memoization)
        {
            if (!memoization.ContainsKey(Index()))
            {
                return false;
            }
            int memo = memoization[Index()];
            Console.WriteLine("parsed list before at index " + Index() +
                               "; skip ahead to token index " + memo + ": " +
                               lookahead[memo].text);
            if (memo == FAILED) throw new PreviousParseFailedException();
            // else skip ahead, pretending we parsed this rule ok
            Seek(memo);
            return true;
        }

        /// <summary>
        /// While backtracking, record partial parsing results.
        /// If invoking rule method failed, record that fact.
        /// If it succeeded, record the token position we should skip to
        /// next time we attempt this rule for this input position.
        /// </summary>
        /// <param name="memoization"></param>
        /// <param name="startTokenIndex"></param>
        /// <param name="failed"></param>
        public void Memoize(Dictionary<int, int> memoization,
                            int startTokenIndex, bool failed)
        {
            // record token just after last in rule if success
            int stopTokenIndex = failed ? FAILED : Index();
            memoization.Add(startTokenIndex, stopTokenIndex);
        }

        /// <summary>
        /// return current input position
        /// </summary>
        /// <returns></returns>
        public int Index()
        { 
            return p;
        }
    }
}