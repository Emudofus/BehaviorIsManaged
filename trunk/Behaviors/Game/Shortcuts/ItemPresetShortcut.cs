using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class ItemPresetShortcut : GeneralShortcut
    {
        public ItemPresetShortcut(PlayedCharacter character, ShortcutObjectPreset shortcut)
            : base(character, shortcut.slot)
        {
            PresetId = shortcut.presetId;
        }

        public int PresetId
        {
            get;
            private set;
        }

        public override void Update(Protocol.Types.Shortcut shortcut)
        {
            base.Update(shortcut);

            if (shortcut is ShortcutObjectPreset)
                PresetId = ( shortcut as ShortcutObjectPreset ).presetId;
        }
    }
}