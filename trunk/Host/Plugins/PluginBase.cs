#region License GNU GPL
// PluginBase.cs
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
using System.IO;
using BiM.Core.Config;
using BiM.Core.Messages;

namespace BiM.Host.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        protected PluginBase(PluginContext context)
        {
            Context = context;
        }

        public abstract bool UseConfig
        {
            get;
        }

        public virtual string ConfigPath
        {
            get { return Path.Combine(GetPluginDirectory(), GenerateConfigName()); }
        }

        public Config Config
        {
            get;
            protected set;
        }

        #region IPlugin Members

        public PluginContext Context
        {
            get;
            protected set;
        }

        public abstract string Name
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public abstract string Author
        {
            get;
        }

        public abstract Version Version
        {
            get;
        }


        public virtual void Initialize()
        {
            MessageDispatcher.RegisterSharedAssembly(Context.PluginAssembly);

            if (UseConfig)
            {
                Config = new Config(ConfigPath);
                Config.BindAssembly(Context.PluginAssembly);
                Config.RegisterAttributes(Context.PluginAssembly);
                Config.Load();
            }
        }

        public virtual void Shutdown()
        {
            MessageDispatcher.UnRegisterSharedAssembly(Context.PluginAssembly);

            if (UseConfig)
            {
                Config.Save();
                Config.UnBindAssembly(Context.PluginAssembly);
            }
        }

        public abstract void Dispose();

        #endregion

        public string GetPluginDirectory()
        {
            return Path.GetDirectoryName(Context.AssemblyPath);
        }

        private string GenerateConfigName()
        {
            return Name.ToLower().Replace(" ", "_") + "_config.xml";
        }
    }
}