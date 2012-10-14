namespace BiM.Behaviors.Settings
{
    public abstract class SettingsEntry
    {
        protected SettingsEntry()
        {
            
        }

        public abstract string EntryName
        {
            get;
        }
    }
}