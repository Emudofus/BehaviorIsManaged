namespace BiM.Core.Messages
{
    public abstract class Message
    {
        public Message()
        {
            Priority = MessagePriority.Normal;
        }

        public abstract uint MessageId
        {
            get;
        }

        public bool Canceled
        {
            get;
            set;
        }

        public MessagePriority Priority
        {
            get;
            set;
        }

        public virtual void BlockProgression()
        {
            Canceled = true;
        }
    }
}