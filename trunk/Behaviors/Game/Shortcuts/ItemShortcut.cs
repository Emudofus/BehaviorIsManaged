using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Types;
using Item = BiM.Behaviors.Game.Items.Item;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class ItemShortcut : GeneralShortcut
    {
        public ItemShortcut(PlayedCharacter character, ShortcutObjectItem shortcut)
            : base(shortcut.slot)
        {
            Item = character.Inventory.GetItem(shortcut.itemUID);
            Template = DataProvider.Instance.Get<Protocol.Data.Item>(shortcut.itemGID);
        }

        public Protocol.Data.Item Template { get; private set; }

        public Item Item
        {
            get;
            private set;
        }
    }
}