using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSyntax
{
    public class Table
    {
        public static readonly object Nil = new Nil();
        
        public string Name { get; set; }
        public Dictionary<Object, Row> Rows { get; set; } = new Dictionary<Object, Row>();
        public List<String> Columns { get; set; } = new List<String>();

        public Table(string name, string primaryKey)
        {
            Name = name;
            AddColumn(primaryKey);
        }

        public void AddColumn(string name)
        {
            Columns.Add(name);
        }

        public void Add(Row r)
        {
            string primaryKey = GetPrimaryKey();
            object primaryKeyValue = r.Values[primaryKey];
            Rows.Add(primaryKeyValue, r);
        }

        public String GetPrimaryKey()
        {
            return Columns[0];
        }

        public override string ToString()
        {
            return "Table{" +
                   "name='" + Name + '\'' +
                   ", rows=" + Rows +
                   ", columns=" + Columns +
                   '}';
        }
    }
}
