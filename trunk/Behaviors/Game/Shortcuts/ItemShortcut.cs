using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Types;
using Item = BiM.Protocol.Data.Item;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class ItemShortcut : GeneralShortcut
    {
        public ItemShortcut(PlayedCharacter character, ShortcutObjectItem shortcut)
            : base(character, shortcut.slot)
        {
            Item = character.Inventory.GetItem(shortcut.itemUID);
            Template = DataProvider.Instance.Get<Item>(shortcut.itemGID);
        }

        public Item Template
        {
            get;
            private set;
        }

        public Items.Item Item
        {
            get;
            private set;
        }

        public override void Update(Protocol.Types.Shortcut shortcut)
        {
            base.Update(shortcut);

            if (shortcut is ShortcutObjectItem)
            {
                Item = Character.Inventory.GetItem(( shortcut as ShortcutObjectItem ).itemUID);
                Template = DataProvider.Instance.Get<Item>(( shortcut as ShortcutObjectItem ).itemGID);
            }
        }
    }
}