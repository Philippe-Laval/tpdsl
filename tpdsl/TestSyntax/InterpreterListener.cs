using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antlr4.Runtime;

namespace TestSyntax
{
    public class InterpreterListener : IInterpreterListener
    {
        public void Info(String msg) { Console.WriteLine(msg); }
        
        public void Error(String msg) { Console.WriteLine(msg); }
        
        public void Error(String msg, Exception e)
        {
            Error(msg);

            if (e.StackTrace != null)
            {
                Error(e.StackTrace);
            }
        }
        
        public void Error(String msg, IToken t)
        {
            Error($"line {t.Line}:{msg}");
        }
    }
}