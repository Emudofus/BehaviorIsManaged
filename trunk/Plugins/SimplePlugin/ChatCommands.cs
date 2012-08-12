using System;
using BiM.Behaviors;
using BiM.Behaviors.Game.Chat;
using BiM.Core.Config;
using BiM.Core.Extensions;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin
{
    public static class ChatCommands
    {
        [MessageHandler(typeof (ChatClientMultiMessage))]
        public static void HandleChatMessage(Bot bot, ChatClientMultiMessage message)
        {
            // if the client sends ".hello" in the chat
            if (message.content == ".hello")
            {
                // do not send this message to the server
                message.BlockNetworkSend();

                // sends a respond to the client
                // todo : method Say()
                bot.ChatManager.SendMessageToClient(string.Format("Hello {0} you are on sub area {1}",
                    bot.Character.Name, bot.Character.Map.SubArea.Name));
            }
        }
    }
}