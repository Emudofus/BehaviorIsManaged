using System;
using System.Reflection;

namespace BiM.Host.Plugins
{
    public class PluginContext
    {
        public PluginContext(string assemblyPath, Assembly pluginAssembly)
        {
            AssemblyPath = assemblyPath;
            PluginAssembly = pluginAssembly;
        }

        internal void Initialize(Type pluginType)
        {
            Plugin = (IPlugin)Activator.CreateInstance(pluginType, this);

            if (Plugin != null)
            {
                Plugin.Initialize();
            }
        }

        public string AssemblyPath
        {
            get;
            private set;
        }

        public Assembly PluginAssembly
        {
            get;
            private set;
        }

        public IPlugin Plugin
        {
            get;
            private set;
        }

        public override string ToString()
        {
            if (Plugin == null)
            {
                return PluginAssembly.FullName;
            }
            return Plugin.Name + " : " + Plugin.Description;
        }
    }
}