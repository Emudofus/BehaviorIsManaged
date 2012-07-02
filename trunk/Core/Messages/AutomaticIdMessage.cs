namespace BiM.Core.Messages
{
    public abstract class AutomaticIdMessage : Message, IDynamicId
    {
        private bool m_assigned;
        private uint m_id;

        public override uint MessageId
        {
            get
            {
                if (m_assigned)
                    return m_id;

                m_id = MessageIdFinder.FindUniqueId();
                m_assigned = true;

                return m_id;
            }
        }
    }
}