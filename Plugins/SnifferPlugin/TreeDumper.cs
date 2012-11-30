#region License GNU GPL

// TreeDumper.cs
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
using System.Collections.Generic;
using System.Reflection;

namespace SnifferPlugin
{
    public class TreeDumper
    {
        public TreeDumper(object target)
        {
            Target = target;
        }

        public object Target
        {
            get;
            private set;
        }

        public ObjectDumpNode GetDumpTree()
        {
            Type type = Target.GetType();
            var tree = new ObjectDumpNode(type.Name);

            foreach (var node in InternalDump(Target, tree))
            {
                tree.Childrens.Add(node);
            }

            return tree;
        }

        public IEnumerable<ObjectDumpNode> InternalDump(object obj, ObjectDumpNode parent)
        {
            if (obj == null || obj is ValueType || obj is string)
            {
                yield return new ObjectDumpNode(FormatValue(obj), parent);
                yield break;
            }

            var enumerableElement = obj as IEnumerable;
            if (enumerableElement != null)
            {
                int count = 0;
                foreach (object item in enumerableElement)
                {
                    count++;
                    if (item == null || item is ValueType || item is string)
                    {
                        yield return new ObjectDumpNode(FormatValue(item), parent);
                    }
                    else
                    {
                        var node = new ObjectDumpNode(item.GetType().Name, parent);
                        foreach (var child in InternalDump(item, node))
                        {
                            node.Childrens.Add(child);
                        }
                        yield return node;
                    }
                }


                if (count == 0)
                {
                    yield return new ObjectDumpNode("-Empty-", parent);
                    yield break;
                }
            }
            else
            {

                Type type = obj.GetType();

                PropertyInfo[] properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                foreach (PropertyInfo property in properties)
                {
                    object value = property.GetValue(obj, new object[0]);

                    if (value is MulticastDelegate)
                        continue;

                    var node = new ObjectDumpNode(property.Name, parent);
                    foreach (var child in InternalDump(value, node))
                    {
                        node.Childrens.Add(child);
                    } 
                    node.IsProperty = true;
                    yield return node;
                }

                FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                foreach (FieldInfo field in fields)
                {
                    object value = field.GetValue(obj);

                    if (value is MulticastDelegate)
                        continue;

                    var node = new ObjectDumpNode(field.Name, parent);
                    foreach (var child in InternalDump(value, node))
                    {
                        node.Childrens.Add(child);
                    } 
                    yield return node;
                }
            }
        }

        private string FormatValue(object o)
        {
            if (o == null)
                return ("null");

            if (o is DateTime)
                return (((DateTime) o).ToShortDateString());

            if (o is string)
                return string.Format("\"{0}\"", o);

            if (o is ValueType)
                return (o.ToString());

            if (o is IEnumerable)
                return ("...");

            return ("{ }");
        }
    }
}