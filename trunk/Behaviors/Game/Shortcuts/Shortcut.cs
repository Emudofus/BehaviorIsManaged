namespace BiM.Behaviors.Game.Shortcuts
{
    public abstract class Shortcut
    {
        public Shortcut()
        {
            
        }

        public Shortcut(int slot)
        {
            Slot = slot;
        }

        public int Slot
        {
            get;
            set;
        }
    }
}