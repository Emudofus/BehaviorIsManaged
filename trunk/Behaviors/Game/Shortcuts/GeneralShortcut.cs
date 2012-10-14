namespace BiM.Behaviors.Game.Shortcuts
{
    public abstract class GeneralShortcut : Shortcut
    {
        public GeneralShortcut()
        {
            
        }

        public GeneralShortcut(int slot)
            : base (slot)
        {
            
        }

        public bool CanUse()
        {
            return false;
        }

        public bool Use()
        {
            // todo
            return CanUse();
        }
    }
}