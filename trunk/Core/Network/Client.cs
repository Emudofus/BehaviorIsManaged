#region License GNU GPL
// Client.cs
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
using System.Globalization;
using System.Net.Sockets;
using BiM.Core.IO;
using BiM.Core.Messages;
using NLog;

namespace BiM.Core.Network
{
    public abstract class Client : IClient
    {
        private object m_lock = new object();
        private readonly BigEndianReader m_buffer = new BigEndianReader();

        private MessagePart m_currentMessage;

        public event Action<Client, NetworkMessage> MessageReceived;
        public event Action<Client, NetworkMessage> MessageSent;
        public event Action<Client> Disconnected;

        public event Action<Client, LogEventInfo> LogMessage;

        protected virtual void OnMessageReceived(NetworkMessage message)
        {
            var handler = MessageReceived;
            if (handler != null)
                handler(this, message);
        }

        protected virtual void OnMessageSended(NetworkMessage message)
        {
            var handler = MessageSent;
            if (handler != null)
                handler(this, message);
        }

        protected virtual void OnClientDisconnected()
        {
            var handler = Disconnected;
            if (handler != null)
                handler(this);
        }

        public Client(Socket socket)
        {
            Socket = socket;
        }

        public Socket Socket
        {
            get;
            private set;
        }

        public bool Connected
        {
            get
            {
                return Socket != null && Socket.Connected;
            }
        } 
       
        /// <summary>
        /// Last activity as a socket client (last received packet or sent packet)
        /// </summary>
        public DateTime LastActivity
        {
            get;
            private set;
        }

        public abstract IMessageBuilder MessageBuilder
        {
            get;
        }

        public virtual void Log(LogLevel level, string message, params object[] args)
        {
            var log = new LogEventInfo(level, "", CultureInfo.CurrentCulture, message, args);


            var handler = LogMessage;
            if (handler != null)
                handler(this, log);
        }

        public virtual void Send(NetworkMessage message)
        {
            if (Socket == null || !Connected)
                return;

            var args = new SocketAsyncEventArgs();
            args.Completed += OnSendCompleted;
            args.UserToken = message;

            byte[] data;
            using (var writer = new BigEndianWriter())
            {
                message.Pack(writer);
                data = writer.Data;
            }

            args.SetBuffer(data, 0, data.Length);

            if (!Socket.SendAsync(args))
            {
                OnMessageSended(message);
                args.Dispose();
            }

            LastActivity = DateTime.Now;
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            OnMessageSended((NetworkMessage)e.UserToken);
            e.Dispose();
        }

        public virtual void Receive(byte[] data, int offset, int count)
        {
            try
            {
                lock (m_lock)
                {
                    m_buffer.Add(data, offset, count);

                    BuildMessage();
                }
            }
            catch (Exception ex)
            {
                Log(LogLevel.Fatal, "Cannot process receive " + ex);
            }
        }

        private void BuildMessage()
        {
            if (m_buffer.BytesAvailable <= 0)
                return;

            if (m_currentMessage == null)
                m_currentMessage = new MessagePart();

            // if message is complete
            if (m_currentMessage.Build(m_buffer))
            {
                var messageDataReader = new BigEndianReader(m_currentMessage.Data);
                var message = MessageBuilder.BuildMessage((uint)m_currentMessage.MessageId.Value, messageDataReader);

                LastActivity = DateTime.Now;

                OnMessageReceived(message);

                m_currentMessage = null;
                BuildMessage(); // there is maybe a second message in the buffer
            }
        }

        public virtual void Reset(Socket newSocket)
        {
            Disconnect();

            Socket = newSocket;
        }

        public virtual void Disconnect()
        {
            if (Socket != null && Socket.Connected)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                Socket = null;
            }

            OnClientDisconnected();
        }
    }
}