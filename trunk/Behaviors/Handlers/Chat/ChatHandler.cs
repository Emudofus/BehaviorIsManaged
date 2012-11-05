using BiM.Behaviors.Game.Chat;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Chat
{
    public class ChatHandler
    {
        [MessageHandler(typeof (ChatClientMultiMessage))]
        public static void HandleChatClientMultiMessage(Bot bot, ChatClientMultiMessage message)
        {
            bot.SendLocal(new ChatMessageClient(message));
        }

        [MessageHandler(typeof(ChatClientPrivateMessage))]
        public static void HandleChatClientPrivateMessage(Bot bot, ChatClientPrivateMessage message)
        {
            bot.SendLocal(new ChatMessageClient(message));
        }

        [MessageHandler(typeof(ChatServerMessage))]
        public static void HandleChatServerMessage(Bot bot, ChatServerMessage message)
        {
            bot.SendLocal(new ChatMessageServer(message));
        }

        [MessageHandler(typeof(ChatServerCopyMessage))]
        public static void HandleChatServerCopyMessage(Bot bot, ChatServerCopyMessage message)
        {
            bot.SendLocal(new ChatMessageServer(message));
        }

        [MessageHandler(typeof(ChatAdminServerMessage))]
        public static void HandleChatAdminServerMessage(Bot bot, ChatAdminServerMessage message)
        {
            bot.SendLocal(new ChatMessageServer(message));
        }
    }
}