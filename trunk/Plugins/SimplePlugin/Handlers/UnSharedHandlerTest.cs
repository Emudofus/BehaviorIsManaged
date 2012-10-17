using BiM.Behaviors;
using BiM.Behaviors.Frames;
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
            message.Bot.AddFrame(new HandlerClass(message.Bot));
        } 
    }

    public class HandlerClass : Frame<HandlerClass>
    {
        public HandlerClass(Bot bot)
            : base(bot)
        {
        }

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
                bot.RemoveFrame(this);
                message.BlockNetworkSend();
            }
        } 

    }
}