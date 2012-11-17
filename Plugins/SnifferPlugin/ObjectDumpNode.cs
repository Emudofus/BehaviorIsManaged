#region License GNU GPL
// ObjectTreeDump.cs
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnifferPlugin
{
    public class ObjectDumpNode : INotifyPropertyChanged
    {
        private List<ObjectDumpNode> m_childrens = new List<ObjectDumpNode>();


        public ObjectDumpNode(string text)
        {
            Text = text;
            IsVisible = true;
        }

        public ObjectDumpNode(string text, object target)
        {
            Text = text;
            Target = target;
            IsVisible = true;
        }

        public ObjectDumpNode(string text, object target, ObjectDumpNode parent)
        {
            Text = text;
            Target = target;
            Parent = parent;
            IsVisible = true;
        }

        public List<ObjectDumpNode> Childrens
        {
            get
            {
                return m_childrens;
            }
        }


        public ObjectDumpNode Parent
        {
            get;
            private set;
        }

        public bool IsVisible
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public bool IsProperty
        {
            get;
            set;
        }

        public object Target
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void FirePropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ExportToString()
        {
            var builder = new StringBuilder();
            ExportToString(0, builder);
            return builder.ToString();
        }

        private void ExportToString(int level, StringBuilder builder)
        {
            builder.Append(new string(' ', level * 4));
            builder.AppendLine(level == 0 ? string.Format("{{{0}}}", Text) : Text);
            level++;

            if (Childrens.Count > 0)
            {
                builder.AppendLine((new string(' ', (level - 1) * 4)) + "{");
                foreach (var child in Childrens)
                {
                    if (!child.IsVisible)
                        continue;

                    // simple value
                    if (child.Childrens.Count == 1 && child.Childrens[0].Childrens.Count == 0)
                    {
                        builder.Append(new string(' ', level * 4));
                        builder.AppendLine(string.Format("{0}:{1}", child.Text, child.Childrens[0].Text));
                    }
                    else
                    {
                        child.ExportToString(level, builder);
                    }
                }
                builder.AppendLine((new string(' ', (level - 1) * 4)) + "}");
            }
        }
    }
}