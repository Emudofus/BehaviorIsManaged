using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Messages;
using BiM.Core.Messages;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    internal static class WelcomeMessageRegister
    {
        [MessageHandler(typeof(BotAddedMessage))]
        public static void OnBotAdded(object sender, BotAddedMessage message)
        {
            message.Bot.AddFrame(new WelcomeMessage(message.Bot));
        } 
    }

    public class WelcomeMessage : Frame<WelcomeMessage>
    {
        private bool m_messageSent;

        public WelcomeMessage(Bot bot)
            : base(bot)
        {
        }

        [MessageHandler(typeof(GameContextCreateMessage))]
        public void HandleGameContextCreateMessage(Bot bot, GameContextCreateMessage message)
        {
            if (!m_messageSent)
            {
                var settings = bot.Settings.GetEntry<Settings>();

                bot.Character.SendMessage(settings.WelcomeMessage);

                if (settings.WelcomeMessage.EndsWith("!"))
                    settings.WelcomeMessage = settings.WelcomeMessage.Remove(settings.WelcomeMessage.Length - 1, 1);
                else
                    settings.WelcomeMessage += "!";

                m_messageSent = true;
            }
        }
    }
}