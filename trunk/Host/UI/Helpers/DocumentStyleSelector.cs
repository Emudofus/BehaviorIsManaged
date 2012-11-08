#region License GNU GPL
// DocumentStyleSelector.cs
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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BiM.Host.UI.Helpers
{
    public class DocumentStyleSelector : StyleSelector
    {
        private Dictionary<object, Style> m_styles = new Dictionary<object, Style>();
        private Dictionary<Type, Style> m_stylesByType = new Dictionary<Type, Style>();

        public bool HasStyle(object item)
        {
            return m_styles.ContainsKey(item);
        }

        public void AddStyle(object item, Style style)
        {
            if (m_styles.ContainsKey(item))
                throw new Exception(string.Format("Item {0} has already a style", item));

            m_styles.Add(item, style);
        }

        public bool RemoveStyle(object item)
        {
            return m_styles.Remove(item);
        }

        public bool HasStyle(Type type)
        {
            return m_stylesByType.ContainsKey(type);
        }

        public void AddStyle(Type type, Style style)
        {
            if (m_stylesByType.ContainsKey(type))
                throw new Exception(string.Format("Type {0} has already a style", type));

            m_stylesByType.Add(type, style);
        }

        public bool RemoveStyle(Type type)
        {
            return m_stylesByType.Remove(type);
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item == null)
                return base.SelectStyle(item, container);

            Style style;

            if (m_stylesByType.TryGetValue(item.GetType(), out style))
                return style;

            if (m_styles.TryGetValue(item, out style))
                return style;

            return base.SelectStyle(item, container);
        }
    }
}