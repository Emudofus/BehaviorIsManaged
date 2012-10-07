using BiM.Behaviors.Settings;

namespace SimplePlugin
{
    public class Settings : SettingsEntry
    {
        public Settings()
        {
            WelcomeMessage = "Welcome";
        }

        public override string EntryName
        {
            get
            {
                return "SimplePlugin";
            }
        }

        public string WelcomeMessage
        {
            get;
            set;
        }
    }
}