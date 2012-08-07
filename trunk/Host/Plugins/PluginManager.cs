using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using BiM.Core.Extensions;
using BiM.Core.Reflection;
using NLog;

namespace BiM.Host.Plugins
{
    public sealed class PluginManager : Singleton<PluginManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // todo : config
        public static List<string> PluginsPath = new List<string>{"./plugins/"};

        public delegate void PluginContextHandler(PluginContext pluginContext);

        public event PluginContextHandler PluginAdded;

        private void OnPluginAdded(PluginContext pluginContext)
        {
            PluginContextHandler handler = PluginAdded;
            if (handler != null) handler(pluginContext);
        }

        public event PluginContextHandler PluginRemoved;

        private void OnPluginRemoved(PluginContext pluginContext)
        {
            PluginContextHandler handler = PluginRemoved;
            if (handler != null) handler(pluginContext);
        }

        private readonly List<PluginContext> m_pluginContexts = new List<PluginContext>();

        public ReadOnlyCollection<PluginContext> Plugins
        {
            get { return m_pluginContexts.AsReadOnly(); }
        }

        private PluginManager()
        {

        }

        public void LoadAllPlugins()
        {
            foreach (var path in PluginsPath)
            {
                if (!Directory.Exists(path) && !File.Exists(path))
                {
                    logger.Error("Cannot load unexistant plugin path {0}", path);
                    return;
                }

                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                {
                    foreach (var file in Directory.EnumerateFiles(path, "*.dll"))
                    {
                        LoadPlugin(file);
                    }
                }
                else
                    LoadPlugin(path);
            }
        }

        public PluginContext LoadPlugin(string libPath)
        {
            if (!File.Exists(libPath))
                throw new FileNotFoundException("File doesn't exist", libPath);

            if (m_pluginContexts.Any(entry => Path.GetFullPath(entry.AssemblyPath) == Path.GetFullPath(libPath)))
                throw new Exception("Plugin already loaded");

            var asmData = File.ReadAllBytes(libPath);

            Assembly pluginAssembly = Assembly.Load(asmData);
            var pluginContext = new PluginContext(libPath, pluginAssembly);
            bool initialized = false;

            // search the entry point (the class that implements IPlugin)
            foreach (Type pluginType in pluginAssembly.GetTypes())
            {
                if (pluginType.IsPublic && !pluginType.IsAbstract)
                {
                    if (pluginType.HasInterface(typeof(IPlugin)))
                    {
                        if (initialized)
                            throw new PluginLoadException("Found 2 classes that implements IPlugin. A plugin can contains only one");

                        pluginContext.Initialize(pluginType);
                        initialized = true;

                        RegisterPlugin(pluginContext);
                    }
                }
            }

            return pluginContext;
        }

        public void UnLoadPlugin(string name, bool ignoreCase = false)
        {
            var plugin = from entry in m_pluginContexts
                         where entry.Plugin.Name.Equals(name, ignoreCase ?
                            StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
                         select entry;

            foreach (var pluginContext in plugin)
            {
                UnLoadPlugin(pluginContext);
            }
        }

        public void UnLoadPlugin(PluginContext context)
        {
            // we cannot unload the assembly, sadly :/
            context.Plugin.Shutdown();
            context.Plugin.Dispose();

            UnRegisterPlugin(context);
        }

        public void UnLoadAllPlugins()
        {
            foreach (var plugin in Plugins.ToArray())
            {
                UnLoadPlugin(plugin);
            }
        }

        public PluginContext GetPlugin(string name, bool ignoreCase = false)
        {
            var plugins = from entry in m_pluginContexts
                         where entry.Plugin.Name.Equals(name, ignoreCase ?
                            StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
                         select entry;

            return plugins.FirstOrDefault();
        }

        private void RegisterPlugin(PluginContext pluginContext)
        {
            m_pluginContexts.Add(pluginContext);

            OnPluginAdded(pluginContext);
        }

        private void UnRegisterPlugin(PluginContext pluginContext)
        {
            m_pluginContexts.Remove(pluginContext);

            OnPluginRemoved(pluginContext);
        }
    }

    public class PluginLoadException : Exception
    {
        public PluginLoadException(string exception)
            : base(exception)
        {

        }
    }
}