using BiM.Behaviors;
using BiM.Core.Messages;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace SimplePlugin.Handlers
{
    internal static class WelcomeMessageRegister
    {
        static WelcomeMessageRegister()
        {
            BotManager.Instance.BotAdded += OnBotAdded;
        }

        private static void OnBotAdded(BotManager sender, Bot bot)
        {
            bot.RegisterHandler(new WelcomeMessage());
        }
    }

    public class WelcomeMessage
    {
        private static bool m_messageSent;
        [MessageHandler(typeof(GameContextCreateMessage))]
        public static void HandleGameContextCreateMessage(Bot bot, GameContextCreateMessage message)
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