using System;
using BiM.Host.Plugins;

namespace BasicPlugin
{
    public class Plugin : PluginBase
    {
        public Plugin(PluginContext context)
            : base(context)
        {
            if (CurrentPlugin != null)
                throw new Exception("Can be instancied only once");

            CurrentPlugin = this;
        }

        public static Plugin CurrentPlugin
        {
            get;
            private set;
        }

        public override string Name
        {
            get
            {
                return "Basic Plugin";
            }
        }

        public override string Description
        {
            get
            {
                return "Manage basics stuff (inventory, server selection ...)";
            }
        }

        public override string Author
        {
            get
            {
                return "timorem";
            }
        }

        public override Version Version
        {
            get
            {
                return new Version(1, 0);
            }
        }

        public override bool UseConfig
        {
            get
            {
                return true;
            }
        }

        public override void Dispose()
        {
        }
    }
}
