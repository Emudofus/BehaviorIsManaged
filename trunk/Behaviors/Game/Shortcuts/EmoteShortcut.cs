using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class EmoteShortcut : GeneralShortcut
    {
        public EmoteShortcut(PlayedCharacter character, ShortcutEmote shortcut)
            : base(character, shortcut.slot)
        {
            Emote = DataProvider.Instance.Get<Emoticon>(shortcut.emoteId);
        }

        public Emoticon Emote
        {
            get;
            private set;
        }

        public override void Update(Protocol.Types.Shortcut shortcut)
        {
            base.Update(shortcut);

            if (shortcut is ShortcutEmote)
                Emote = DataProvider.Instance.Get<Emoticon>((shortcut as ShortcutEmote).emoteId);
        }
    }
}