using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestFig
{
    public class MyFigParser : FigParser
    {
        public MyFigParser(ITokenStream input) : base(input)
        {
        }

        protected override void SetObjectProperty(object o, string propertyName, object value)
        {
            Type c = o.GetType();

            // First see if name is a property ala javabeans
            string methodSuffix = propertyName.Substring(0, 1).ToUpper() + propertyName.Substring(1);
            MethodInfo? m = GetMethod(c, "set" + methodSuffix, new Type[] { value.GetType() });
            if (m != null)
            {
                try
                {
                    InvokeMethod(m, o, value);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Can't set property " + propertyName + " using method set" + methodSuffix +
                        " from " + c.FullName + " instance: " + e);
                }
            }
            else
            {
                //// try for a visible field
                //try
                //{
                //    Field f = c.getField(propertyName);
                //    try
                //    {
                //        f.set(o, value);
                //    }
                //    catch (IllegalAccessException iae)
                //    {
                //        System.err.println("Can't access property " + propertyName + " using direct field access from " +
                //                c.getName() + " instance: " + iae);
                //    }
                //}
                //catch (NoSuchFieldException nsfe)
                //{
                //    System.err.println("Class " + c.getName() + " has no such property/field: " + propertyName +
                //        ": " + nsfe);
                //}
            }
        }

        protected override object? NewInstance(string name)
        {
            object? o = null;

            try
            {
                Assembly asm = typeof(Server).Assembly;
                Type? type = asm.GetType($"TestFig.{name}");
                if (type is not null)
                {
                    o = Activator.CreateInstance(type);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("can't make instance of " + name);
            }

            return o;
        }

        protected override MethodInfo? GetMethod(Type c, string methodName, Type[] args)
        {
            MethodInfo? m;
            try
            {
                m = c.GetMethod(methodName, args);
            }
            catch (Exception)
            {
                m = null;
            }
            return m;
        }

        protected override object? InvokeMethod(MethodInfo m, object? o)
        {
            return m.Invoke(o, null);
        }

        protected override object? InvokeMethod(MethodInfo m, object? o, object? value)
        {
            object?[]? args = null;
            if (value != null)
            {
                args = new object?[] { value };
            }
            value = m.Invoke(o, args);
            return value;
        }
    }
}
