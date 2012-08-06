using BiM.Core.Messages;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Handlers
{
    public class ConnectionHandler
    {
        [MessageHandler(typeof(HelloConnectMessage))]
        public static void HandleHelloConnectMessage(Bot bot, HelloConnectMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(IdentificationMessage))]
        public static void HandleIdentificationMessage(Bot bot, IdentificationMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(IdentificationFailedBannedMessage))]
        public static void HandleIdentificationFailedBannedMessage(Bot bot, IdentificationFailedBannedMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(IdentificationSuccessMessage))]
        public static void HandleIdentificationSuccessMessage(Bot bot, IdentificationSuccessMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(IdentificationFailedForBadVersionMessage))]
        public static void HandleIdentificationFailedForBadVersionMessage(Bot bot, IdentificationFailedForBadVersionMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(SelectedServerDataMessage))]
        public static void HandleSelectedServerDataMessage(Bot bot, SelectedServerDataMessage message)
        {
            bot.ClientInformations.Update(message);
        }

        [MessageHandler(typeof(ServersListMessage))]
        public static void HandleServersListMessage(Bot bot, ServersListMessage message)
        {
            bot.ClientInformations.Update(message);
        }
    }
}