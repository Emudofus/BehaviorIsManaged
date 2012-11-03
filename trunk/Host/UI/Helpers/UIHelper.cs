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