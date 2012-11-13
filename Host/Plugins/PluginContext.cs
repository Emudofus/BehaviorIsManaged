#region License GNU GPL
// PluginContext.cs
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