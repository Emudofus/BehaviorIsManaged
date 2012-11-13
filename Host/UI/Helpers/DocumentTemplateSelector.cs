#region License GNU GPL
// DocumentTemplateSelector.cs
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
    public class DocumentTemplateSelector : DataTemplateSelector
    {
        private Dictionary<object, DataTemplate> m_templates = new Dictionary<object, DataTemplate>();
        private Dictionary<Type, DataTemplate> m_templatesByType = new Dictionary<Type, DataTemplate>();

        public bool HasTemplate(object item)
        {
            return m_templates.ContainsKey(item);
        }

        public void AddTemplate(object item, DataTemplate template)
        {
            if (m_templates.ContainsKey(item))
                throw new Exception(string.Format("Item {0} has already a template", item));

            m_templates.Add(item, template);
        }

        public bool RemoveTemplate(object item)
        {
            return m_templates.Remove(item);
        }

        public bool HasTemplate(Type type)
        {
            return m_templatesByType.ContainsKey(type);
        }

        public void AddTemplate(Type type, DataTemplate template)
        {
            if (m_templatesByType.ContainsKey(type))
                throw new Exception(string.Format("Type {0} has already a template", type));

            m_templatesByType.Add(type, template);
        }

        public bool RemoveTemplate(Type type)
        {
            return m_templatesByType.Remove(type);
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return base.SelectTemplate(item, container);

            DataTemplate template;

            if (m_templatesByType.TryGetValue(item.GetType(), out template))
                return template;

            if (m_templates.TryGetValue(item, out template))
                return template;

            return base.SelectTemplate(item, container);
        }
    }
}