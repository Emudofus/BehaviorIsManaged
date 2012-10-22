using System;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Enums;
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

        public override ShortcutBarEnum BarType
        {
            get { return ShortcutBarEnum.SPELL_SHORTCUT_BAR; }
        }

        public void Add(ShortcutSpell shortcut)
        {
            Add(new SpellShortcut(Character, shortcut));
        }

        public override void Update(ShortcutBarContentMessage content)
        {
            if (content == null) throw new ArgumentNullException("content");
            Clear();
            foreach (Protocol.Types.Shortcut shortcut in content.shortcuts)
            {
                if (shortcut is ShortcutSpell)
                    Add(shortcut as ShortcutSpell);
            }
        }

        public override void Update(ShortcutBarRefreshMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if ((ShortcutBarEnum)message.barType != BarType)
                return;

            var shortcut = Get(message.shortcut.slot);
            if (shortcut != null)
                shortcut.Update(message.shortcut);
        }
    }
}