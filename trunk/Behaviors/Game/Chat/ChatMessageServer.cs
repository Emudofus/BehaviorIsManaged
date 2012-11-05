using System;
using System.Linq;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Actors.Interfaces;
using BiM.Behaviors.Game.World;
using BiM.Core.Extensions;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Game.Chat
{
    public class ChatMessageServer : ChatMessage
    {
        public ChatMessageServer()
        {
            
        }

        public ChatMessageServer(ChatServerMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Content = message.content;
            Channel = (ChatActivableChannelsEnum)message.channel;
            SentTime = message.timestamp.UnixTimestampToDateTime();
            FingerPrint = message.fingerprint;
            SenderId = message.senderId;
            SenderName = message.senderName;
            SenderAccountId = message.senderAccountId;
        }

        public ChatMessageServer(ChatServerCopyMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Content = message.content;
            Channel = (ChatActivableChannelsEnum)message.channel;
            SentTime = message.timestamp.UnixTimestampToDateTime();
            ReceiverId = message.receiverId;
            ReceiverName = message.receiverName;
            FingerPrint = message.fingerprint;
            SenderId = ReceiverId;
            SenderName = ReceiverName;
            Copy = true;
        }

        public ChatMessageServer(ChatAdminServerMessage message)
            : this((ChatServerMessage)message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Admin = true;
        }

        public ContextActor TryGetSender(IContext context)
        {
            return context.Actors.FirstOrDefault(entry => ( entry is INamed && ( entry as INamed ).Name == SenderName ) || entry.Id == SenderId);
        }

        public DateTime SentTime
        {
            get;
            set;
        }

        public string FingerPrint
        {
            get;
            set;
        }

        public int SenderId
        {
            get;
            set;
        }

        public string SenderName
        {
            get;
            set;
        }

        public int SenderAccountId
        {
            get;
            set;
        }

        public bool Copy
        {
            get;
            set;
        }

        public int ReceiverId
        {
            get;
            set;
        }

        public string ReceiverName
        {
            get;
            set;
        }

        public bool Admin
        {
            get;
            set;
        }
    }
}