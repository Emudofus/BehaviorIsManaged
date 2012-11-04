using System;
using System.IO;
using System.Reflection;

namespace BiM.Host.UI.Helpers
{
    /// <summary>
    /// This class provides information about the running application.
    /// </summary>
    public static class ApplicationInfo
    {
        private static string m_productName;
        private static bool m_productNameCached;
        private static string m_version;
        private static bool m_versionCached;
        private static string m_company;
        private static bool m_companyCached;
        private static string m_copyright;
        private static bool m_copyrightCached;
        private static string m_applicationPath;
        private static bool m_applicationPathCached;


        /// <summary>
        /// Gets the product name of the application.
        /// </summary>
        public static string ProductName
        {
            get
            {
                if (!m_productNameCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        AssemblyProductAttribute attribute = ( (AssemblyProductAttribute)Attribute.GetCustomAttribute(
                            entryAssembly, typeof(AssemblyProductAttribute)) );
                        m_productName = ( attribute != null ) ? attribute.Product : "";
                    }
                    else
                    {
                        m_productName = "";
                    }
                    m_productNameCached = true;
                }
                return m_productName;
            }
        }

        /// <summary>
        /// Gets the version number of the application.
        /// </summary>
        public static string Version
        {
            get
            {
                if (!m_versionCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        m_version = entryAssembly.GetName().Version.ToString();
                    }
                    else
                    {
                        m_version = "";
                    }
                    m_versionCached = true;
                }
                return m_version;
            }
        }

        /// <summary>
        /// Gets the company of the application.
        /// </summary>
        public static string Company
        {
            get
            {
                if (!m_companyCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        AssemblyCompanyAttribute attribute = ( (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(
                            entryAssembly, typeof(AssemblyCompanyAttribute)) );
                        m_company = ( attribute != null ) ? attribute.Company : "";
                    }
                    else
                    {
                        m_company = "";
                    }
                    m_companyCached = true;
                }
                return m_company;
            }
        }

        /// <summary>
        /// Gets the copyright information of the application.
        /// </summary>
        public static string Copyright
        {
            get
            {
                if (!m_copyrightCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        AssemblyCopyrightAttribute attribute = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
                            entryAssembly, typeof(AssemblyCopyrightAttribute));
                        m_copyright = attribute != null ? attribute.Copyright : "";
                    }
                    else
                    {
                        m_copyright = "";
                    }
                    m_copyrightCached = true;
                }
                return m_copyright;
            }
        }

        /// <summary>
        /// Gets the path for the executable file that started the application, not including the executable name.
        /// </summary>
        public static string ApplicationPath
        {
            get
            {
                if (!m_applicationPathCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        m_applicationPath = Path.GetDirectoryName(entryAssembly.Location);
                    }
                    else
                    {
                        m_applicationPath = "";
                    }
                    m_applicationPathCached = true;
                }
                return m_applicationPath;
            }
        }
    }
}