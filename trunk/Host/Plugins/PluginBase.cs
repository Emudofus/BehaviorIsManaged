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