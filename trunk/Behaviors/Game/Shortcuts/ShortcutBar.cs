using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Game.Shortcuts
{
    public class ShortcutBar
    {
        public ShortcutBar(PlayedCharacter character)
        {
            Owner = character;
        }

        public ShortcutBar(PlayedCharacter character, ShortcutBarContentMessage message)
        {
            Owner = character;
            // todo
        }

        public PlayedCharacter Owner
        {
            get;
            set;
        }
    }
}