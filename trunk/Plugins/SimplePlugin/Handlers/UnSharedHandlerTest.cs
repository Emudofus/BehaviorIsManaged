using BiM.Behaviors;
using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    public class UnSharedHandlerTest
    {
        static UnSharedHandlerTest()
        {
            BotManager.Instance.BotAdded += OnBotAdded;
        }

        public static void OnBotAdded(BotManager sender, Bot bot)
        {
            bot.RegisterHandler(new HandlerClass());
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
                bot.UnRegisterHandler(this);
                message.BlockNetworkSend();
            }
        } 

    }
}