/***
 * Excerpted from "Language Implementation Patterns",
 * published by The Pragmatic Bookshelf.
 * Copyrights apply to this code. It may not be used to create training material, 
 * courses, books, articles, and the like. Contact us if you are in doubt.
 * We make no guarantees that this code is fit for any purpose. 
 * Visit http://www.pragmaticprogrammer.com/titles/tpdsl for more book information.
***/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestReg
{
    public class LabelSymbol
    {
        public string Name { get; set; }

        /// <summary>
        /// Address in code memory
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Is this ref'd before def'd.
        /// </summary>
        public bool IsForwardRef { get; set; } = false;

        /// <summary>
        /// Set when we see actual ID: definition
        /// </summary>
        public bool IsDefined { get; set; } = true;

        /// <summary>
        /// List of operands in memory we need to update after seeing def
        /// </summary>
        public List<int>? ForwardReferences { get; set; } = null;


        public LabelSymbol(String name)
        {
            Name = name;
        }

        public LabelSymbol(String name, int address)
            : this(name)
        {
            Address = address;
        }

        public LabelSymbol(String name, int address, bool forward)
            : this(name)
        {
            IsForwardRef = forward;
            if (forward)
            {
                // if forward reference, then address is address to update
                AddForwardReference(address);
            }
            else
            {
                Address = address;
            }
        }

        public void AddForwardReference(int address)
        {
            if (ForwardReferences == null)
            {
                ForwardReferences = new List<int>();
            }
            ForwardReferences.Add(address);
        }

        public void ResolveForwardReferences(byte[] code)
        {
            IsForwardRef = false;
            // Need to patch up all references to this symbol
            List<int>? opndsToPatch = ForwardReferences;
            if (opndsToPatch is not null)
            {
                foreach (int addrToPatch in opndsToPatch)
                {
                    /*
                    Console.WriteLine("updating operand at addr "+
                            addr+" to be "+getAddress());
                    */
                    BytecodeAssembler.WriteInt(code, addrToPatch, Address);
                }
            }
        }

        public override string ToString()
        {
            String refs = "";
            if (ForwardReferences != null)
            {
                refs = "[refs=" + ForwardReferences.ToString() + "]";
            }
            return Name + "@" + Address + refs;
        }
    }
}
