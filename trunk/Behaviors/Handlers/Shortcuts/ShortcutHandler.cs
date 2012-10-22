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

        [MessageHandler(typeof (ShortcutBarRefreshMessage))]
        public static void HandleShortcutBarRefreshMessage(Bot bot, ShortcutBarRefreshMessage message)
        {
            if (message.barType == (int)bot.Character.GeneralShortcuts.BarType)
                bot.Character.GeneralShortcuts.Update(message);
            else
                bot.Character.SpellShortcuts.Update(message);
        }

        [MessageHandler(typeof(ShortcutBarRemovedMessage))]
        public static void HandleShortcutBarRemovedMessage(Bot bot, ShortcutBarRemovedMessage message)
        {
            if (message.barType == (int) bot.Character.GeneralShortcuts.BarType)
                bot.Character.GeneralShortcuts.Remove(message.slot);
            else
                bot.Character.SpellShortcuts.Remove(message.slot);

        }
    }
}