namespace BiM.Core.Messages
{
    public abstract class Message
    {
        public abstract uint MessageId
        {
            get;
        }

        public bool Canceled
        {
            get;
            set;
        }

        public void BlockProgression()
        {
            Canceled = true;
        }
    }
}