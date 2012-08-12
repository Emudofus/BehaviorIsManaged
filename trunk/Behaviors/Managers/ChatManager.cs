using System;
using BiM.Core.Config;
using BiM.Core.Extensions;
using BiM.Core.Network;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;
using NLog;

namespace BiM.Behaviors.Managers
{
    public class ChatManager
    {
        private Bot _bot;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ChatManager(Bot bot)
        {
            this._bot = bot;
        }

        public ChatActivableChannelsEnum defaultChannel
        {
            get
            {
                return Config.GetStatic("DefaultChannel", ChatActivableChannelsEnum.CHANNEL_ADS);
            }
        }

        public string defaultSenderName
        {
            get
            {
                return Config.GetStatic("DefaultSenderName", "BiM");
            }
        }

        #region SendToClient
        /// <summary>
        /// Send a Message to the D. Client
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToClient(string content)
        {
            this.SendMessageToClient(content, defaultSenderName, defaultChannel);
        }

        /// <summary>
        /// Send a Message to the D. Client
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        /// <param name="senderName">Name of the Sender</param>
        public void SendMessageToClient(string content, string senderName)
        {
            this.SendMessageToClient(content, senderName, defaultChannel);
        }

        /// <summary>
        /// Send a Message to the D. Client
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        /// <param name="senderName">Name of the Sender</param>
        /// <param name="channel">Channel of the chat Message</param>
        public void SendMessageToClient(string content, string senderName, ChatActivableChannelsEnum channel, string LogMessage = "Message Sent to Client")
        {
            this._bot.SendToClient(new ChatServerMessage((sbyte)channel, content, (int)DateTime.Now.DateTimeToUnixTimestamp(), "", 0, senderName, 0));
            logger.Debug(LogMessage);
        }

        /// <summary>
        /// Send an Error Message to the D. Client
        /// </summary>
        /// <param name="content">Content of error Message</param>
        public void SendErrorToClient(string content)
        {
            this.SendMessageToClient("[Error]" + content, defaultSenderName, ChatActivableChannelsEnum.CHANNEL_ADMIN, "Error message Sent to Client");
        }

        /// <summary>
        /// Send an Information Message to the D. Client
        /// </summary>
        /// <param name="content">Content of info Message</param>
        public void SendInfoToClient(string content)
        {
            this.SendMessageToClient("[Info]" + content, defaultSenderName, ChatActivableChannelsEnum.CHANNEL_ALIGN, "Info message Sent to Client");
        }
        #endregion

        #region SendToServer
        /// <summary>
        /// Send a chat message to current Map
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToMap(string content)
        {
            this.SendMessage(content, ChatActivableChannelsEnum.CHANNEL_GLOBAL);
        }

        /// <summary>
        /// Send a chat message to the sales channel(Commerce)
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToSales(string content)
        {
            this.SendMessage(content, ChatActivableChannelsEnum.CHANNEL_SALES);
        }

        /// <summary>
        /// Send a chat message to the party channel(Groupe)
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToParty(string content)
        {
            this.SendMessage(content, ChatActivableChannelsEnum.CHANNEL_PARTY);
        }

        /// <summary>
        /// Send a chat message to the seek channel(Recrutement)
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToSeek(string content)
        {
            this.SendMessage(content, ChatActivableChannelsEnum.CHANNEL_SEEK);
        }

        /// <summary>
        /// Send a chat message to the guild channel(Guilde)
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToGuild(string content)
        {
            this.SendMessage(content, ChatActivableChannelsEnum.CHANNEL_GUILD);
        }

        /// <summary>
        /// Send a private message to one player
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        /// <param name="receiverName">Name of the receiver</param>
        public void SendPrivateMessage(string content, string receiverName)
        {
            this.SendMessage(new ChatClientPrivateMessage(content, receiverName));
        }

        /// <summary>
        /// Send a message to a specific channel
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        /// <param name="channel">Channel of the message</param>
        public void SendMessage(string content, ChatActivableChannelsEnum channel)
        {
            this.SendMessage(new ChatClientMultiMessage(content, (sbyte)channel));
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="message">Network Message</param>
        public void SendMessage(NetworkMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            _bot.SendToServer(message);

            if (message is ChatClientPrivateMessage)
                logger.Debug("Private Message Sent to Server");
            else
                logger.Debug("Message Sent to Server");
        }
        #endregion
    }
}