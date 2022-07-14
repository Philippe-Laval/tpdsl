using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonolithic
{
    /// <summary>
    ///  A "tag" to indicate which symbols are types
    /// </summary>
    public interface IType
    {
        public string Name { get; }
    }
}
