using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antlr4.Runtime;

namespace TestSyntax
{
    /// <summary>
    /// How to response to messages and errors from interpreter
    /// </summary>
    public interface IInterpreterListener
    {
        public void Info(String msg);
        public void Error(String msg);
        public void Error(String msg, Exception e);
        public void Error(String msg, IToken t);
    }
}
