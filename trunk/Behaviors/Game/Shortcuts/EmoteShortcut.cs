using BiM.Behaviors.Data;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Data;
using BiM.Protocol.Types;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class EmoteShortcut : GeneralShortcut
    {
        public EmoteShortcut(PlayedCharacter character, ShortcutEmote shortcut)
            : base(shortcut.slot)
        {
            Emote = DataProvider.Instance.Get<Emoticon>(shortcut.emoteId);
        }

        public Emoticon Emote { get; set; }
    }
}