using BiM.Core.Messages;

namespace BiM.Behaviors.Messages
{
    public class BotRemovedMessage : Message
    {
        public BotRemovedMessage(Bot bot)
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