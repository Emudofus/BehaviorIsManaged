#region License GNU GPL
// ObjectDumper.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace BiM.Core.Reflection
{
    public class ObjectDumper
    {
        private readonly int m_indentSize;
        private readonly bool m_fields;
        private readonly bool m_properties;
        private readonly BindingFlags m_binding;

        public ObjectDumper(int indentSize, bool fields, bool properties, BindingFlags binding = BindingFlags.Default)
        {
            m_indentSize = indentSize;
            m_fields = fields;
            m_properties = properties;
            m_binding = binding;
        }

        public string Dump(object obj)
        {
            return InternalDump(obj, new StringBuilder(), 0);
        }

        private string InternalDump(object element, StringBuilder stringBuilder, int level)
        {
            if (element == null || element is ValueType || element is string)
            {
                Write(FormatValue(element), stringBuilder, level);
            }
            else
            {
                var objectType = element.GetType();
                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    Write("{{{0}}}", stringBuilder, level, objectType.FullName);
                    level++;
                }

                var enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    var count = 0;
                    foreach (object item in enumerableElement)
                    {
                        count++;

                        if (item is IEnumerable && !( item is string ))
                        {
                            level++;
                            InternalDump(item, stringBuilder, level);
                            level--;
                        }
                        else
                        {
                            InternalDump(item, stringBuilder, level);
                        }
                    }

                    if (count == 0)
                        Write("-Empty-", stringBuilder, level);
                }
                else
                {
                    if (m_fields)
                    {
                        var fields = element.GetType().GetFields(m_binding);
                        foreach (var field in fields)
                        {
                            object value = field.GetValue(element);

                            //events
                            if (value is MulticastDelegate)
                                continue;

                            if (field.FieldType.IsValueType || field.FieldType == typeof (string))
                            {
                                Write("{0}: {1}", stringBuilder, level, field.Name, FormatValue(value));
                            }
                            else
                            {
                                Write("{0}: {1}", stringBuilder, level, field.Name, typeof(IEnumerable).IsAssignableFrom(field.FieldType) ? "..." : "{ }");
                                level++;
                                InternalDump(value, stringBuilder, level);
                                level--;
                            }
                        }
                    }

                    if (m_properties)
                    {
                        var properties = element.GetType().GetProperties(m_binding);
                        foreach (var property in properties)
                        {
                            object value = property.GetValue(element, null);

                            //events
                            if (value is MulticastDelegate)
                                continue;

                            if (property.PropertyType.IsValueType || property.PropertyType == typeof (string))
                            {
                                Write("{0}: {1}", stringBuilder, level, property.Name, FormatValue(value));
                            }
                            else
                            {
                                Write("{0}: {1}", stringBuilder, level, property.Name, typeof(IEnumerable).IsAssignableFrom(property.PropertyType) ? "..." : "{ }");
                                level++;
                                InternalDump(value, stringBuilder, level);
                                level--;
                            }

                        }
                    }
                }

                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    level--;
                }
            }

            return stringBuilder.ToString();
        }

        private void Write(string value, StringBuilder builder, int level, params object[] args)
        {
            var space = new string(' ', level * m_indentSize);

            if (args != null)
                value = string.Format(value, args);

            builder.AppendLine(space + value);
        }

        private string FormatValue(object o)
        {
            if (o == null)
                return ( "null" );

            if (o is DateTime)
                return ( ( (DateTime)o ).ToShortDateString() );

            if (o is string)
                return string.Format("\"{0}\"", o);

            if (o is ValueType)
                return ( o.ToString() );

            if (o is IEnumerable)
                return ( "..." );

            return ( "{ }" );
        }
    }
}