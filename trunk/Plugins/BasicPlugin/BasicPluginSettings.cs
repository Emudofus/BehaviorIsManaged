using BasicPlugin.Chat;
using BiM.Behaviors.Settings;

namespace BasicPlugin
{
    public class BasicPluginSettings : SettingsEntry
    {
        public BasicPluginSettings()
        {
            FloodEntries = new FloodEntry[0];
        }

        public override string EntryName
        {
            get
            {
                return "BasicPlugin";
            }
        }

        public FloodEntry[] FloodEntries
        {
            get;
            set;
        }
    }
}