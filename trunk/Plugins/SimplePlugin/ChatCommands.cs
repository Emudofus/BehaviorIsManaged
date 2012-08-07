using System;
using BiM.Behaviors;
using BiM.Behaviors.Game.Chat;
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
            // if the client send ".hello" in the chat
            if (message.content == ".hello")
            {
                // do not send this message to the server
                message.BlockNetworkSend();

                // send a respond to the client
                // todo : method Say()
                bot.SendToClient(new ChatServerMessage(message.channel, "Hello, my name is BiM", (int) DateTime.Now.DateTimeToUnixTimestamp(), "", 0, "BiM", 0));
            }
        }
    }
}