using BiM.Behaviors;
using BiM.Behaviors.Messages;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    public class UnSharedHandlerTest
    {
        [MessageHandler(typeof(BotAddedMessage))]
        public static void OnBotAdded(object sender, BotAddedMessage message)
        {
            message.Bot.AddHandler(new HandlerClass());
        } 
    }

    public class HandlerClass
    {
        [MessageHandler(typeof(ChatClientMultiMessage))]
        public void HandleChatClientMultiMessage(Bot bot, ChatClientMultiMessage message)
        {
            if (message.content == ".test")
            {
                bot.Character.OpenPopup("Yes Man !");
                message.BlockNetworkSend();
            }
            else if (message.content == ".nop")
            {
                bot.RemoveHandler(this);
                message.BlockNetworkSend();
            }
        } 

    }
}