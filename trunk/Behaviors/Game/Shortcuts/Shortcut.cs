using BiM.Behaviors.Game.Actors.RolePlay;

namespace BiM.Behaviors.Game.Shortcuts
{
    public abstract class Shortcut
    {
        public Shortcut(PlayedCharacter character, int slot)
        {
            Character = character;
            Slot = slot;
        }

        public PlayedCharacter Character
        {
            get;
            protected set;
        }

        public int Slot
        {
            get;
            protected set;
        }
    }
}