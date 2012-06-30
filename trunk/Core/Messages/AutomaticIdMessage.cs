namespace BiM.Core.Messages
{
    public abstract class AutomaticIdMessage : Message
    {
        public override uint MessageId
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }
    }
}