using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Shortcuts
{
    public class ShortcutHandler
    {
        [MessageHandler(typeof(ShortcutBarContentMessage))]
        public static void HandleShortcutBarContentMessage(Bot bot, ShortcutBarContentMessage message)
        {
            bot.Character.Update(message);
        }
    }
}