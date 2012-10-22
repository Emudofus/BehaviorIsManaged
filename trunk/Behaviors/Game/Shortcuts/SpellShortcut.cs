using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Spells;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class SpellShortcut : Shortcut
    {
        public SpellShortcut(PlayedCharacter character, ShortcutSpell shortcut)
            : base(character, shortcut.slot)
        {
            Character = character;
            SpellId = shortcut.spellId;
        }

        public int SpellId
        {
            get;
            private set;
        }

        public Spell GetSpell()
        {
            return Character.SpellsBook.GetSpell(SpellId);
        }

        public void Update(Protocol.Types.Shortcut shortcut)
        {
            Slot = shortcut.slot;
            if (shortcut is ShortcutSpell)
            {
                SpellId = (shortcut as ShortcutSpell).spellId;
            }
        }
    }
}