using System;
using System.IO;
using BiM.Core.Messages;

namespace BiM.Host.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        protected PluginBase(PluginContext context)
        {
            Context = context;
        }

        public PluginContext Context
        {
            get;
            protected set;
        }

        #region IPlugin Members

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
            MessageDispatcher.RegisterAssembly(Context.PluginAssembly);
        }

        public virtual void Shutdown()
        {
            MessageDispatcher.UnRegisterAssembly(Context.PluginAssembly);
        }

        public abstract void Dispose();

        #endregion

        public string GetPluginDirectory()
        {
            return Path.GetDirectoryName(Context.AssemblyPath);
        }
    }
}