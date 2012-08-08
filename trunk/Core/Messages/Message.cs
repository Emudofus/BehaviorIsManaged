using System;
using System.Threading;

namespace BiM.Core.Messages
{
    [Serializable]
    public abstract class Message
    {
        public event Action<Message> Dispatched;
        private bool m_dispatched;

        public Message()
        {
            Priority = MessagePriority.Normal;
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

        public bool IsDispatched
        {
            get { return m_dispatched; }
        }

        public virtual void BlockProgression()
        {
            Canceled = true;
        }

        public void OnDispatched()
        {
            if (m_dispatched)
                return;

            lock (this)
            {
                Monitor.PulseAll(this);
            }

            m_dispatched = true;

            var evnt = Dispatched;
            if (evnt != null)
                evnt(this);
        }

        /// <summary>
        /// Block the current thread until the message is dispatched
        /// </summary>
        /// <returns>false whenever the message has already been dispatched</returns>
        public bool Wait()
        {
            return Wait(TimeSpan.Zero);
        }
        
        /// <summary>
        /// Block the current thread until the message is dispatched
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns>false whenever the message has already been dispatched</returns>
        public bool Wait(TimeSpan timeout)
        {
            if (m_dispatched)
                return false;

            lock (this)
            {
                if (timeout > TimeSpan.Zero)
                    Monitor.Wait(this, timeout);
                else
                    Monitor.Wait(this);
            }

            return true;
        }
    }
}