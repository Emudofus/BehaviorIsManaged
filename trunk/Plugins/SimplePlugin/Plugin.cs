using System;
using BiM.Host.Plugins;
using SimplePlugin.Handlers;

namespace SimplePlugin
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
            get { return "Simple Plugin"; }
        }

        public override string Description
        {
            get { return "Just an example"; }
        }

        public override string Author
        {
            get { return "timorem"; }
        }

        public override Version Version
        {
            get { return new Version(1, 0); }
        }

        public override bool UseConfig
        {
            get { return true; }
        }

        public override void Initialize()
        {
            base.Initialize();
            GameCommands.CreateTchatCommand("Seek", "Seek <NomDuMonstre>", MonsterSeeker.HandleSeekCommand);
            GameCommands.CreateTchatCommand("Stop", "Stop <NomDuMonstre>", MonsterSeeker.HandleStopCommand);
            GameCommands.CreateTchatCommand("List", "List", MonsterSeeker.HandleListCommand);
        }

        public override void Dispose()
        {
        }
    }
}