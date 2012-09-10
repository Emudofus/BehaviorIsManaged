using System;
using BiM.Core.Extensions;
using BiM.Core.Network;

namespace BiM.Core.Messages
{
    public class MessageHandlerAttribute : Attribute
    {
        public MessageHandlerAttribute(Type type)
        {
            MessageType = type;
        }

        public Type MessageType
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

        public Type FilterType { get; set; }
    }
}