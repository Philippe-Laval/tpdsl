using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSyntax
{
    public class ResultSet
    {
        public List<List<object>> Results { get; set; } = new List<List<object>>();

        public void Add(List<object> row)
        {
            Results.Add(row);
        }
    }
}
