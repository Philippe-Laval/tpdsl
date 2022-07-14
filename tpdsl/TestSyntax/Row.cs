using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antlr4.Runtime;

namespace TestSyntax
{
    public class Row
    {
        public Dictionary<string, object> Values { get; set; } = new Dictionary<String, Object>();

        public Row(List<string> columns)
        {
            foreach (string c in columns) Values.Add(c, Table.Nil);
        }

        public List<object> GetColumns()
        {
            List<object> row = new List<object>();
            foreach (object o in Values.Values) row.Add(o);
            return row;
        }

        public List<object> GetColumns(IList<IToken> columns)
        {
            List<object> row = new List<object>();
            foreach (IToken t in columns) row.Add(Values[t.Text]);
            return row;
        }

        public void Set(string col, object value)
        {
            if (Values.ContainsKey(col))
            {
                Values[col] = value;
            }
            else
            {
                Values.Add(col, value);
            }
        }

        public override string ToString()
        {
            string result = string.Join(", ", Values.Select(o => $"{o.Key}={o.Value}"));
            return result;
        }
    }
}
