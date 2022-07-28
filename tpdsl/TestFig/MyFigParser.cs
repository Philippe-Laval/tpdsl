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

        protected override void SetObjectProperty(object obj, string propertyName, object value)
        {
            Type type = obj.GetType();

            // First see if name is a property ala javabeans
            string methodSuffix = propertyName.Substring(0, 1).ToUpper() + propertyName.Substring(1);
            MethodInfo? methodInfo = GetMethod(type, "Set" + methodSuffix, new Type[] { value.GetType() });
            if (methodInfo != null)
            {
                try
                {
                    InvokeMethod(methodInfo, obj, value);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Can't set property " + propertyName + " using method set" + methodSuffix +
                        " from " + type.FullName + " instance: " + e);
                }
            }
            else
            {
                // https://stackoverflow.com/questions/25757121/c-sharp-how-to-set-propertyinfo-value-when-its-type-is-a-listt-and-i-have-a-li
                // propertyInfo.SetValue(obj, value.Cast<int>().ToList(), null);


                // try for a visible field
                try
                {
                    string name = propertyName.Substring(0, 1).ToUpper() + propertyName.Substring(1);
                    PropertyInfo? propertyInfo = type.GetProperty(name);
                    if (propertyInfo is not null)
                    {
                        try
                        {

                            Type type2 = propertyInfo.PropertyType;
                            if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                Type itemType = type2.GetGenericArguments()[0]; // use this...

                                // IEnumerable<TResult> Cast<TResult>(this IEnumerable source);
                                MethodInfo? CastMethod = typeof(Enumerable).GetMethod("Cast");
                                List<string> a;
                                // List<TSource> ToList<TSource>(this IEnumerable<TSource> source);
                                MethodInfo? ToListMethod = typeof(Enumerable).GetMethod("ToList");


                                var castItems = CastMethod?.MakeGenericMethod(new Type[] { itemType })
                                                            .Invoke(null, new object[] { value });
                                if (castItems is not null)
                                {
                                    var list = ToListMethod?.MakeGenericMethod(new Type[] { itemType })
                                                              .Invoke(null, new object[] { castItems });
                                    if (list is not null)
                                    {
                                        propertyInfo.SetValue(obj, list, null);
                                    }
                                }

                                // Works to
                                //
                                //var listType = typeof(List<>);
                                //var genericArgs = propertyInfo.PropertyType.GetGenericArguments();
                                //var concreteType = listType.MakeGenericType(genericArgs);
                                //var instance = Activator.CreateInstance(concreteType);
                                //if (instance is System.Collections.IList newList)
                                //{
                                //    if (value is List<object> oldList)
                                //    {
                                //        foreach (object o in oldList)
                                //        {
                                //            newList.Add(o);
                                //        }

                                //        propertyInfo.SetValue(obj, newList, null);
                                //    }
                                //}
                            }
                            else
                            {
                                propertyInfo.SetValue(obj, value, null);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Can't access property " + name + " using direct property access from " +
                                    type.FullName + " instance: " + e);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Class " + type.FullName + " has no such property/field: " + name);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
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

        protected MethodInfo? GetMethod(Type c, string methodName, Type[] args)
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

        protected object? InvokeMethod(MethodInfo m, object? o)
        {
            return m.Invoke(o, null);
        }

        protected object? InvokeMethod(MethodInfo m, object? o, object? value)
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
