using System;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class GeneralShortcutBar : ShortcutBar<GeneralShortcut>
    {
        public GeneralShortcutBar(PlayedCharacter character)
            : base(character)
        {
        }

        public void Add(ShortcutObjectItem item)
        {
            Add(new ItemShortcut(Character, item));
        }

        public void Add(ShortcutObjectPreset shortcut)
        {
            Add(new ItemPresetShortcut(Character, shortcut));
        }

        public void Add(ShortcutEmote shortcut)
        {
            Add(new EmoteShortcut(Character, shortcut));
        }

        public void Add(ShortcutSmiley shortcut)
        {
            Add(new SmileyShortcut(Character, shortcut));
        }

        public void Add(Protocol.Types.Shortcut shortcut)
        {
            if (shortcut is ShortcutObjectItem)
                Add(shortcut as ShortcutObjectItem);
            else if (shortcut is ShortcutObjectPreset)
                Add(shortcut as ShortcutObjectPreset);
            else if (shortcut is ShortcutEmote)
                Add(shortcut as ShortcutEmote);
            else if (shortcut is ShortcutSmiley)
                Add(shortcut as ShortcutSmiley);
        }

        public void Update(ShortcutBarContentMessage content)
        {
            if (content == null) throw new ArgumentNullException("content");
            Clear();
            foreach (var shortcut in content.shortcuts)
            {
                Add(shortcut);
            }
        }
    }
}