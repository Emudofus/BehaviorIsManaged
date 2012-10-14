using System;
using BiM.Behaviors.Game.Actors.RolePlay;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class SpellShortcutBar : ShortcutBar
    {
        public SpellShortcutBar(PlayedCharacter character)
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