using System;

namespace BiM.Core.Messages
{
    public class MessageHandlerAttribute : Attribute
    {
        public uint MessageId
        {
            get;
            set;
        }
    }
}