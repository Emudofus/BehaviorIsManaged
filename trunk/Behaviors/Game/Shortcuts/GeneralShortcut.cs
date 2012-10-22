using BiM.Behaviors.Game.Actors.RolePlay;

namespace BiM.Behaviors.Game.Shortcuts
{
    public abstract class GeneralShortcut : Shortcut
    {
        public GeneralShortcut(PlayedCharacter character, int slot)
            : base(character, slot)
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

        public virtual void Update(Protocol.Types.Shortcut shortcut)
        {
            Slot = shortcut.slot;
        }
    }
}