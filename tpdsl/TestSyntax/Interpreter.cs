using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSyntax
{
    public class Interpreter
    {
        // default response to messages
        public InterpreterListener listener = new InterpreterListener();

        public Dictionary<string, object> Globals { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, Table> Tables { get; set; } = new Dictionary<string, Table>();

        public void Interp(TextReader input) {
            QLexer lex = new QLexer(new AntlrInputStream(input));
            CommonTokenStream tokens = new CommonTokenStream(lex);
            QParser parser = new QParser(tokens, this);
            parser.program();
            // System.out.println(tables);
        }

        public void CreateTable(String name,
                                String primaryKey,
                                IList<IToken> columns)
        {
            Table table = new Table(name, primaryKey);
            foreach (IToken t in columns) table.AddColumn(t.Text);
            Tables.Add(name, table);
        }

        public void InsertInto(String name, Row row)
        {
            Table t = Tables[name];
            if (t == null) { listener.Error("No such table " + name); return; }
            t.Add(row);
        }

        public Object Select(String name, IList<IToken> columns)
        {
            Table table = Tables[name];
            ResultSet rs = new ResultSet();
            foreach (Row r in table.Rows.Values)
            {
                rs.Add(r.GetColumns(columns));
            }
            return rs;
        }

        public object Select(string name, IList<IToken> columns, string key, object value)
        {
            Table table = Tables[name];
            ResultSet rs = new ResultSet();
            
            if (key.Equals(table.GetPrimaryKey()))
            {
                List<object> selectedColumnData = table.Rows[value].GetColumns(columns);
                if (selectedColumnData.Count == 1) return selectedColumnData[0];
                rs.Add(selectedColumnData);
                return rs;
            }

            // key isn't the primary key; walk linearly to find all rows satisfying
            foreach (Row r in table.Rows.Values) 
            {
                if (r.Values[key].Equals(value))
                {
                    rs.Add(r.GetColumns(columns));
                }
            }

            return rs;
        }

        public void Store(string name, Object o) 
        {
            Globals.Add(name, o); 
        }

        public object Load(string name) 
        {
            return Globals[name]; 
        }

        public void Print(object o)
        {
            if (o != null)
            {
                // result set?
                if (o is ResultSet)
                {
                    ResultSet rs = (ResultSet)o;

                    foreach (List<object> r in rs.Results)
                    {
                        for (int i = 0; i < r.Count; i++)
                        {
                            if (i > 0) Console.Write(", ");
                            Console.Write(r[i]);
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine(o.ToString());
                }
            }
        }

    }
}
