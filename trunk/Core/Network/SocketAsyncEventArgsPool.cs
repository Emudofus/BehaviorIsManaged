#region License GNU GPL
// SocketAsyncEventArgsPool.cs
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