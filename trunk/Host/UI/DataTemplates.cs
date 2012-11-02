using System;
using System.Windows;
using BiM.Core.Reflection;

namespace BiM.Host.UI
{
    public static class DataTemplates
    {
        private static ResourceDictionary m_resources = new ResourceDictionary();

        static DataTemplates()
        {
            m_resources.Source = new Uri("/BiM.Host;component/UI/DataTemplates.xaml", UriKind.RelativeOrAbsolute);
        }

        public static DataTemplate GetTemplate(string name)
        {
            return m_resources[name] as DataTemplate;
        }
    }
}