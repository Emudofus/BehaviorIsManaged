using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Types;
using Item = BiM.Behaviors.Game.Items.Item;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class ItemPresetShortcut : GeneralShortcut
    {
        public ItemPresetShortcut(PlayedCharacter character, ShortcutObjectPreset shortcut)
            : base(shortcut.slot)
        {
            PresetId = shortcut.presetId;
        }

        public int PresetId { get; set; }
    }
}