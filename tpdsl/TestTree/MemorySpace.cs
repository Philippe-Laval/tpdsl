using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    /// <summary>
    /// A scope of variable:value pairs
    /// </summary>
    public class MemorySpace
    {
        /// <summary>
        /// The name of the MemorySpace (mainly for debugging purposes)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The members of the MemorySpace
        /// </summary>
        public Dictionary<string, object?> Members { get; set; } = new Dictionary<string, object?>();

        public MemorySpace(string name)
        {
            Name = name;
        }

        public bool ContainsMember(string id)
        {
            if (Members.ContainsKey(id))
            {
                return true;
            }

            return false;
        }

        public object? Get(string id)
        {
            if (Members.ContainsKey(id))
            {
                return Members[id];
            }

            return null;
        }

        /// <summary>
        /// Add or overwrite the value for the member with name "id"
        /// </summary>
        /// <param name="id">The name of the member</param>
        /// <param name="value">The value of the member</param>
        public void Put(string id, object? value)
        {
            if (!Members.ContainsKey(id))
            {
                Members.Add(id, value);
            }
            else
            {
                Members[id] = value;
            }
        }

        public override string ToString()
        {
            var temp = Members.Select(o => $"{o.Key}={o.Value?.ToString() ?? "null"}");
            var result = String.Join(", ", temp);

            return $"{Name}:{temp}";
        }
    }
}
