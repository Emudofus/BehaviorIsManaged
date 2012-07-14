using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace BiM.Core.Network
{
    public sealed class BufferManager : IDisposable
    {
        private readonly int m_bufferSize;
        private readonly int m_numBytes; // the total number of bytes controlled by the buffer pool
        private byte[] m_buffer; // the underlying byte array maintained by the Buffer Manager
        private int m_currentIndex;
        private Stack<int> m_freeIndexPool;

        public BufferManager(int totalBytes, int bufferSize)
        {
            m_numBytes = totalBytes;
            m_currentIndex = 0;
            m_bufferSize = bufferSize;
            m_freeIndexPool = new Stack<int>();
        }

        #region IDisposable Members

        public void Dispose()
        {
            m_buffer = null;
            m_freeIndexPool = null;
        }

        #endregion

        // Allocates buffer space used by the buffer pool
        public void InitializeBuffer()
        {
            // create one big large buffer and divide that 
            // out to each SocketAsyncEventArg object
            m_buffer = new byte[m_numBytes];
        }

        // Assigns a buffer from the buffer pool to the 
        // specified SocketAsyncEventArgs object
        //
        // <returns>true if the buffer was successfully set, else false</returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
            }
            else
            {
                if ((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    return false;
                }
                args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                m_currentIndex += m_bufferSize;
            }
            return true;
        }

        // Removes the buffer from a SocketAsyncEventArg object.  
        // This frees the buffer back to the buffer pool
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}