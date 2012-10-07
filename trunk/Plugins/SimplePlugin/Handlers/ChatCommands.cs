using BiM.Behaviors;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    public static class ChatCommands
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            // if the client sends ".hello" in the chat
            if (message.content == ".hello")
            {
                message.BlockNetworkSend();// do not send this message to the server

                bot.Character.SendMessage(string.Format("Hello {0} you are on sub area {1}",
                    bot.Character.Name, bot.Character.Map.SubArea.Name));
            }
        }
    }
}