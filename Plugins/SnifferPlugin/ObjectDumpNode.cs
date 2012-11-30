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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System;
using BiM.Core.Network;

namespace SnifferPlugin
{
  public class ObjectDumpNode : INotifyPropertyChanged
  {
    private ObservableCollection<ObjectDumpNode> m_childrens = new ObservableCollection<ObjectDumpNode>();


    /*public ObjectDumpNode(string text)
    {
      Name = text;
      IsVisible = true;
    }*/

    /*public ObjectDumpNode(string text, object target)
    {
      Name = text;
      //Target = target;
      IsVisible = true;
    }*/

    public ObjectDumpNode(string text, ObjectDumpNode parent = null)
    {
      Name = text;
      //Target = target;
      Parent = parent;
      IsVisible = true;
    }

    public ObservableCollection<ObjectDumpNode> Childrens
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

    public override string ToString()
    {
        if (TimeStamp != null)
            return string.Format(" [{0}] {1} ({2}) - {3}", TimeStamp, Name, Id, From);
        else
            return Name;
    }

    public ListenerEntry? From
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }

    public uint Id
    {
        get;
        set;
    }

    public DateTime? TimeStamp
    {
        get;
        set;
    }

    public bool IsProperty
    {
      get;
      set;
    }

    // Used in Xaml binding
    public string Text
    {
        get { return ToString(); }
    }

    // Problem: Target keep a useless reference on message, preventing GC
    /*public object Target
    {
      get;
      set;
    }*/

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public string ExportToString(bool toCSV)
    {
      var builder = new StringBuilder();
      ExportToString(0, builder, toCSV);
      return builder.ToString();
    }

    const string CSVSeparator = ";";

    private void AddSpace(StringBuilder builder, int level, bool toCSV)
    {
      if (toCSV)          
        builder.Append(new string(';', level+4));
      else
        builder.Append(new string(' ', level * 4));
    }

    private void ExportToString(int level, StringBuilder builder, bool toCSV)
    {
      if (level > 0) AddSpace(builder, level, toCSV);
      if (level == 0 && toCSV && TimeStamp.HasValue)
          builder.AppendLine(TimeStamp.Value.ToLongTimeString() + CSVSeparator + Name + CSVSeparator + Id + CSVSeparator + From);
      else
          builder.AppendLine(level == 0 && !toCSV ? string.Format("{{{0}}}", ToString()) : ToString());
      level++;

      if (Childrens.Count > 0)
      {
        if (!toCSV)
        {
          AddSpace(builder, level - 1, toCSV);
          builder.AppendLine("{");
        }
        foreach (var child in Childrens)
        {
          if (!child.IsVisible)
            continue;

          // simple value
          if (child.Childrens.Count == 1 && child.Childrens[0].Childrens.Count == 0)
          {
            AddSpace(builder, level, toCSV);
            if (toCSV)
                builder.AppendLine(string.Format("{0}{1}{2}", child, CSVSeparator, child.Childrens[0]));
              else
                builder.AppendLine(string.Format("{0}:{1}", child, child.Childrens[0]));
          }
          else
          {
            child.ExportToString(level, builder, toCSV);
          }
        }
        if (!toCSV)
        {
          AddSpace(builder, level - 1, toCSV);
          builder.AppendLine("}");
        }
      }
    }
  }
}