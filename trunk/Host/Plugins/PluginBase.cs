using System;
using System.IO;

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
        }

        public virtual void Shutdown()
        {
        }

        public abstract void Dispose();

        #endregion

        public string GetPluginDirectory()
        {
            return Path.GetDirectoryName(Context.AssemblyPath);
        }
    }
}