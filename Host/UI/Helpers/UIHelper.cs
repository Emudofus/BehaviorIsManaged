#region License GNU GPL
// UIHelper.cs
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
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace BiM.Host.UI.Helpers
{
    public class UIHelper
    {
        public static IList<DependencyProperty> GetAttachedProperties(Object element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var attachedProperties = new List<DependencyProperty>();

            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(element,
                                                                           new Attribute[]
                                                                               {
                                                                                   new PropertyFilterAttribute(PropertyFilterOptions.SetValues |
                                                                                                               PropertyFilterOptions.UnsetValues |
                                                                                                               PropertyFilterOptions.Valid)
                                                                               }))
            {
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pd);
                if (dpd != null && dpd.IsAttached)
                {
                    attachedProperties.Add(dpd.DependencyProperty);
                }
            }

            return attachedProperties;
        }

        public static IList<DependencyProperty> GetProperties(Object element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var properties = new List<DependencyProperty>();

            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(element,
                                                                           new Attribute[]
                                                                               {
                                                                                   new PropertyFilterAttribute(PropertyFilterOptions.SetValues |
                                                                                                               PropertyFilterOptions.UnsetValues |
                                                                                                               PropertyFilterOptions.Valid)
                                                                               }))
            {
                DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pd);
                if (dpd != null)
                {
                    properties.Add(dpd.DependencyProperty);
                }
            }

            return properties;
        }

        public static IEnumerable<Binding> EnumerateBindings(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            LocalValueEnumerator lve = element.GetLocalValueEnumerator();

            while (lve.MoveNext())
            {
                LocalValueEntry entry = lve.Current;

                if (BindingOperations.IsDataBound(element, entry.Property))
                {
                    Binding binding = (entry.Value as BindingExpression).ParentBinding;
                    yield return binding;
                }
            }
        }
    }
}