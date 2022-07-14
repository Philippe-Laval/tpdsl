using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTree
{
    /// <summary>
    /// A scope holding fields of a struct instance.  
    /// There can be multiple struct instances but only one StructSymbol(definition).
    /// </summary>
    public class StructInstance : MemorySpace
    {
        /// <summary>
        /// what kind of struct am I?
        /// </summary>
        public StructSymbol Def { get; set; }

        /// <summary>
        /// Create a StructInstance object with the definition of the struct (StructSymbol)
        /// </summary>
        /// <param name="structSymbol"></param>
        public StructInstance(StructSymbol structSymbol)
            : base(structSymbol.GetName() + " instance")
        {
            Def = structSymbol;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("{");
            bool first = true;
            foreach (string fieldName in Def.Fields.Keys)
            {
                object? v = null;

                if (Members.ContainsKey(fieldName))
                {
                    v = Members[fieldName];
                }
                
                if (!first) buf.Append(", ");
                else first = false;
                
                buf.Append(fieldName);
                buf.Append('=');
                buf.Append(v?.ToString() ?? "null");
            }
            buf.Append("}");
            return buf.ToString();
        }

    }
}
