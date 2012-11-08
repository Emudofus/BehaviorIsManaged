#region License GNU GPL
// Message.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
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

        /// <summary>
        /// Internal use only
        /// </summary>
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