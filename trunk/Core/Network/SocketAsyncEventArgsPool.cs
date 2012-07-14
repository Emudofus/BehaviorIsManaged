using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace BiM.Core.Network
{
    public sealed class SocketAsyncEventArgsPool : IDisposable
    {
        private Stack<SocketAsyncEventArgs> m_pool;
        private bool m_disposed;

        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        ///   Gets the number of SocketAsyncEventArgs instances in the pool
        /// </summary>
        public int Count
        {
            get { return m_pool.Count; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_pool.Clear();
            m_disposed = true;
        }

        #endregion

        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }

        public SocketAsyncEventArgs Pop()
        {
            if (m_disposed)
                throw new ObjectDisposedException("SocketAsyncEventArgsPool");

            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }
    }
}