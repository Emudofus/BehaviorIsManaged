using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BiM.Host.Plugins;

namespace SimplePlugin
{
    public class Plugin : PluginBase
    {
        public Plugin(PluginContext context)
            : base(context)
        {
        }

        public override string Name
        {
            get
            {
                return "Simple Plugin";
            }
        }

        public override string Description
        {
            get
            {
                return "Just an example";
            }
        }

        public override string Author
        {
            get { return "timorem"; }
        }

        public override Version Version
        {
            get { return new Version(1,0); }
        }

        public override void Dispose()
        {
            
        }
    }
}
