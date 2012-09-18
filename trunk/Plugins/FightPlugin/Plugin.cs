using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiM.Host.Plugins;

namespace FightPlugin
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
                return "Fight Plugin";
            }
        }

        public override string Description
        {
            get
            {
                return "Manage fights";
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
