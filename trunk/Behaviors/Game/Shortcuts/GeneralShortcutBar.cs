using System;
using BiM.Behaviors.Game.Actors.RolePlay;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class GeneralShortcutBar : ShortcutBar
    {
        public GeneralShortcutBar(PlayedCharacter character)
        {
            if (character == null) throw new ArgumentNullException("character");
            Character = character;
        }

        public PlayedCharacter Character
        {
            get;
            set;
        }
    }
}