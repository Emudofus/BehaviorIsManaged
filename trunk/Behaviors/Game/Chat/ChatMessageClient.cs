using System;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BiM.Behaviors.Game.Chat
{
    public class ChatMessageClient : ChatMessage
    {
        public ChatMessageClient()
        {
            
        }

        public ChatMessageClient(ChatClientPrivateMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Content = message.content;
            ReceiverName = message.receiver;
            Channel = ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE;
        }

        public ChatMessageClient(ChatClientMultiMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Content = message.content;
            Channel = (ChatActivableChannelsEnum) message.channel;
        }

        public string ReceiverName
        {
            get;
            set;
        }
    }
}