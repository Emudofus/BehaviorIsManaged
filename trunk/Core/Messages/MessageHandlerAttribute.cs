using System;
using BiM.Core.Network;

namespace BiM.Core.Messages
{
    public class MessageHandlerAttribute : Attribute
    {
        public MessageHandlerAttribute(uint id)
        {
            MessageId = id;
        }

        public uint MessageId
        {
            get;
            set;
        }

        public ListenerEntry FromFilter
        {
            get;
            set;
        }

        public ListenerEntry DestinationFilter
        {
            get;
            set;
        }
    }
}