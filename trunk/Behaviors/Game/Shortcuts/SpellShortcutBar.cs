using System;
using System.Collections.ObjectModel;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class SpellShortcutBar : ShortcutBar<SpellShortcut>
    {
        public SpellShortcutBar(PlayedCharacter character)
            : base(character)
        {
        }

        public void Add(ShortcutSpell shortcut)
        {
            Add(new SpellShortcut(Character, shortcut));
        }

        public void Update(ShortcutBarContentMessage content)
        {
            if (content == null) throw new ArgumentNullException("content");
            Clear();
            foreach (var shortcut in content.shortcuts)
            {
                if (shortcut is ShortcutSpell)
                    Add(shortcut as ShortcutSpell);
            }
        }
    }
}