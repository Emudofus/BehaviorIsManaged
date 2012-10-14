using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Spells;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class SpellShortcut : Shortcut
    {
        public SpellShortcut(PlayedCharacter character, ShortcutSpell shortcut)
            : base(shortcut.slot)
        {
            Character = character;
            SpellId = shortcut.spellId;
        }

        public PlayedCharacter Character { get; private set; }

        public int SpellId { get; private set; }

        public Spell GetSpell()
        {
            return Character.SpellsBook.GetSpell(SpellId);
        }
    }
}