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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Bot m_bot;

        public ChatManager(Bot bot)
        {
            m_bot = bot;
        }

        [Configurable("DefaultChannel")]
        public static ChatActivableChannelsEnum DefaultChannel = ChatActivableChannelsEnum.CHANNEL_ADS;

        [Configurable("DefaultSenderName")]
        public static string DefaultSenderName = "BiM";

        #region SendToClient

        /// <summary>
        /// Send a Message to the Client
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToClient(string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            SendMessageToClient(content, DefaultSenderName, DefaultChannel);
        }

        /// <summary>
        /// Send a Message to the Client
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        /// <param name="senderName">Name of the Sender</param>
        public void SendMessageToClient(string content, string senderName)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (senderName == null) throw new ArgumentNullException("senderName");
            SendMessageToClient(content, senderName, DefaultChannel);
        }

        /// <summary>
        /// Send a Message to the Client
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        /// <param name="senderName">Name of the Sender</param>
        /// <param name="channel">Channel of the chat Message</param>
        public void SendMessageToClient(string content, string senderName, ChatActivableChannelsEnum channel)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (senderName == null) throw new ArgumentNullException("senderName");
            m_bot.SendToClient(new ChatServerMessage((sbyte) channel, content, (int) DateTime.Now.DateTimeToUnixTimestamp(), "", 0, senderName, 0));
        }

        /// <summary>
        /// Send an Error Message to the Client
        /// </summary>
        /// <param name="content">Content of error Message</param>
        public void SendErrorToClient(string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            SendMessageToClient("[Error]" + content, DefaultSenderName, ChatActivableChannelsEnum.CHANNEL_ADMIN);
        }

        /// <summary>
        /// Send an Information Message to the Client
        /// </summary>
        /// <param name="content">Content of info Message</param>
        public void SendInfoToClient(string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            SendMessageToClient("[Info]" + content, DefaultSenderName, ChatActivableChannelsEnum.CHANNEL_ALIGN);
        }

        #endregion

        #region SendToServer

        /// <summary>
        /// Send a chat message to current Map
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToMap(string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            SendMessage(content, ChatActivableChannelsEnum.CHANNEL_GLOBAL);
        }

        /// <summary>
        /// Send a chat message to the sales channel(Commerce)
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToSales(string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            SendMessage(content, ChatActivableChannelsEnum.CHANNEL_SALES);
        }

        /// <summary>
        /// Send a chat message to the party channel(Groupe)
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToParty(string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            SendMessage(content, ChatActivableChannelsEnum.CHANNEL_PARTY);
        }

        /// <summary>
        /// Send a chat message to the seek channel(Recrutement)
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToSeek(string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            SendMessage(content, ChatActivableChannelsEnum.CHANNEL_SEEK);
        }

        /// <summary>
        /// Send a chat message to the guild channel(Guilde)
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        public void SendMessageToGuild(string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            SendMessage(content, ChatActivableChannelsEnum.CHANNEL_GUILD);
        }

        /// <summary>
        /// Send a private message to one player
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        /// <param name="receiverName">Name of the receiver</param>
        public void SendPrivateMessage(string content, string receiverName)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (receiverName == null) throw new ArgumentNullException("receiverName");
            SendMessage(new ChatClientPrivateMessage(content, receiverName));
        }

        /// <summary>
        /// Send a message to a specific channel
        /// </summary>
        /// <param name="content">Content of chat Message</param>
        /// <param name="channel">Channel of the message</param>
        public void SendMessage(string content, ChatActivableChannelsEnum channel)
        {
            if (content == null) throw new ArgumentNullException("content");
            SendMessage(new ChatClientMultiMessage(content, (sbyte) channel));
        }

        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="message">Network Message</param>
        public void SendMessage(NetworkMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            m_bot.SendToServer(message);

            if (message is ChatClientPrivateMessage)
                logger.Debug("Private Message Sent to Server");
            else
                logger.Debug("Message Sent to Server");
        }

        #endregion
    }
}