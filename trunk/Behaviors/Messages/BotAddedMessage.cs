using BiM.Core.Messages;

namespace BiM.Behaviors.Messages
{
    public class BotAddedMessage : Message
    {
        public BotAddedMessage(Bot bot)
        {
            Bot = bot;
        }

        public Bot Bot
        {
            get;
            set;
        }
    }
}