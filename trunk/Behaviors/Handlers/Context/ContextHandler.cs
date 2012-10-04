using BiM.Core.Messages;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Context
{
    public class ContextHandler
    {
        [MessageHandler(typeof(GameContextCreateMessage))]
        public static void HandleGameContextCreateMessage(Bot bot, GameContextCreateMessage message)
        {
            if (bot.Display != DisplayState.InGame)
                bot.Display = DisplayState.InGame;

            bot.Character.ChangeContext((GameContextEnum)message.context);
        }

        [MessageHandler(typeof(GameContextDestroyMessage))]
        public static void HandleGameContextDestroyMessage(Bot bot, GameContextDestroyMessage message)
        {
            bot.Character.LeaveContext();
        }

        [MessageHandler(typeof (GameContextRemoveElementMessage))]
        public static void HandleGameContextRemoveElementMessage(Bot bot, GameContextRemoveElementMessage message)
        {
            // can be on the map or in fight
            bot.Character.Context.RemoveContextActor(message.id);
        }
    }
}