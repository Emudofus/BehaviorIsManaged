using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers.Basics
{
    public static class BasicHandler
    {
        [MessageHandler(typeof(BasicTimeMessage))]
        public static void HandleBasicTimeMessage(Bot bot, BasicTimeMessage message)
        {
            bot.ClientInformations.Update(message);
        }
    }
}