#region License GNU GPL
// PluginManager.cs
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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using BiM.Core.Config;
using BiM.Core.Extensions;
using BiM.Core.Reflection;
using NLog;

namespace BiM.Host.Plugins
{
    public sealed class PluginManager : Singleton<PluginManager>
    {
        #region Delegates

        public delegate void PluginContextHandler(PluginContext pluginContext);

        #endregion

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Configurable("PluginsFolder")]
        public static string PluginsFolder = "./plugins";

        private readonly List<PluginContext> m_pluginContexts = new List<PluginContext>();

        private PluginManager()
        {
        }

        public ReadOnlyCollection<PluginContext> Plugins
        {
            get { return m_pluginContexts.AsReadOnly(); }
        }

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

        public void LoadAllPlugins()
        {
            if (!Directory.Exists(PluginsFolder))
            {
                Directory.CreateDirectory(PluginsFolder);
            }

            foreach (string file in Directory.EnumerateFiles(PluginsFolder, "*.dll"))
            {
                LoadPlugin(file);
            }
        }

        public PluginContext LoadPlugin(string libPath)
        {
            if (!File.Exists(libPath))
                throw new FileNotFoundException("File doesn't exist", libPath);

            if (m_pluginContexts.Any(entry => Path.GetFullPath(entry.AssemblyPath) == Path.GetFullPath(libPath)))
                throw new Exception("Plugin already loaded");

            byte[] asmData = File.ReadAllBytes(libPath);

            Assembly pluginAssembly = Assembly.Load(asmData);
            var pluginContext = new PluginContext(libPath, pluginAssembly);
            bool initialized = false;

            // search the entry point (the class that implements IPlugin)
            foreach (Type pluginType in pluginAssembly.GetTypes())
            {
                if (pluginType.IsPublic && !pluginType.IsAbstract)
                {
                    if (pluginType.HasInterface(typeof (IPlugin)))
                    {
                        if (initialized)
                            throw new PluginLoadException("Found 2 classes that implements IPlugin. A plugin can contains only one");

                        pluginContext.Initialize(pluginType);
                        initialized = true;

                        RegisterPlugin(pluginContext);
                    }
                }
            }

            if (initialized)
                logger.Info("Plugin '{0}' loaded", pluginContext.Plugin.Name);
            else
                logger.Error("Plugin {0} has no IPlugin entry point", pluginContext.PluginAssembly.GetName().Name);

            return pluginContext;
        }

        public void UnLoadPlugin(string name, bool ignoreCase = false)
        {
            IEnumerable<PluginContext> plugin = from entry in m_pluginContexts
                                                where entry.Plugin.Name.Equals(name, ignoreCase
                                                                                         ? StringComparison.InvariantCultureIgnoreCase
                                                                                         : StringComparison.InvariantCulture)
                                                select entry;

            foreach (PluginContext pluginContext in plugin)
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
            foreach (PluginContext plugin in Plugins.ToArray())
            {
                UnLoadPlugin(plugin);
            }
        }

        public PluginContext GetPlugin(string name, bool ignoreCase = false)
        {
            IEnumerable<PluginContext> plugins = from entry in m_pluginContexts
                                                 where entry.Plugin.Name.Equals(name, ignoreCase
                                                                                          ? StringComparison.InvariantCultureIgnoreCase
                                                                                          : StringComparison.InvariantCulture)
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