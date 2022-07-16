using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hetero
{
    public class Token
    {
        public static readonly int INVALID_TOKEN_TYPE = 0;
        public static readonly int PLUS = 1; // token types
        public static readonly int INT = 2;
        public static readonly int VECT = 3;

        public int Type { get; set; }
        public string Text { get; set; } = string.Empty;

        public Token(int type, string text)
        {
            this.Type = type;
            this.Text = text;
        }

        public Token(int type)
        {
            this.Type = type;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
