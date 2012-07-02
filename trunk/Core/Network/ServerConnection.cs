using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using BiM.Core.IO;
using BiM.Core.Messages;
using NLog;

namespace BiM.Core.Network
{
    public class ServerConnection : IClient
    {
        private readonly string m_host;
        private readonly int m_port;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly BigEndianReader m_buffer = new BigEndianReader();
        private MessagePart m_currentMessage;
        private readonly object m_recvLocker = new object();

        private IPEndPoint m_endPoint;
        public event Action<ServerConnection, NetworkMessage> MessageReceived;
        public event Action<ServerConnection, NetworkMessage> MessageSended;
        public event Action<ServerConnection> Connected;
        public event Action<ServerConnection> Disconnected;

        public event Action<ServerConnection, LogEventInfo> LogMessage;

        private void OnMessageReceived(NetworkMessage message)
        {
            var handler = MessageReceived;
            if (handler != null)
                handler(this, message);
        }

        private void OnMessageSended(NetworkMessage message)
        {
            var handler = MessageSended;
            if (handler != null)
                handler(this, message);
        }

        private void OnClientConnected()
        {
            var handler = Connected;
            if (handler != null)
                handler(this);
        }

        private void OnClientDisconnected()
        {
            var handler = Disconnected;
            if (handler != null)
                handler(this);
        }

        public ServerConnection(string host, int port, IMessageBuilder messageBuilder)
        {
            m_host = host;
            m_port = port;
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IP = string.Format("{0}:{1}", host, port);
            MessageBuilder = messageBuilder;
        }

        public Socket Socket
        {
            get;
            private set;
        }

        public bool IsConnected
        {
            get { return Socket != null && Socket.Connected; }
        }

        public string IP
        {
            get;
            private set;
        }

        /// <summary>
        /// Last activity as a socket client (since last packet received sent )
        /// </summary>
        public DateTime LastActivity
        {
            get;
            private set;
        }

        public IMessageBuilder MessageBuilder
        {
            get;
            private set;
        }

       

        public void Log(LogLevel level, string message, params object[] args)
        {
            var log = new LogEventInfo(level, "", CultureInfo.CurrentCulture, message, args);


            var handler = LogMessage;
            if (handler != null)
                handler(this, log);
        }

        public void Connect()
        {
            if (Socket == null)
            {
                Log(LogLevel.Fatal, "Socket already closed");
                return;
            }

            Socket.Connect(m_host, m_port);
            OnClientConnected();

            ReceiveLoop();
        }

        public void Reconnect()
        {
            if (IsConnected)
                Disconnect();

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Connect();
        }

        /// <summary>
        ///   Disconnect the Client
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected)
            {
                Close();
            }

            OnClientDisconnected();
        }

        public void Send(NetworkMessage message)
        {
            if (!IsConnected)
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

        private void OnSendCompleted(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            OnMessageSended((NetworkMessage)socketAsyncEventArgs.UserToken);
            socketAsyncEventArgs.Dispose();
        }

        private void ReceiveLoop()
        {
            if (!IsConnected)
                return;
            
            var args = new SocketAsyncEventArgs();
            args.Completed += OnReceiveCompleted;
            args.SetBuffer(new byte[8192], 0, 8192);

            if (!Socket.ReceiveAsync(args))
            {
                ProcessReceiveCompleted(args);
            }
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceiveCompleted(args);
                    break;
                case SocketAsyncOperation.Disconnect:
                    Disconnect();
                    break;
            }
        }

        private void ProcessReceiveCompleted(SocketAsyncEventArgs args)
        {
            if (!IsConnected)
                return;

            if (args.BytesTransferred <= 0 ||
                args.SocketError != SocketError.Success)
            {
                Disconnect();
            }
            else
            {
                Receive(args.Buffer, args.Offset, args.BytesTransferred);

                ReceiveLoop();
            }

        }

        public virtual void Receive(byte[] data, int offset, int count)
        {
            lock (m_recvLocker)
            {
                m_buffer.Add(data, offset, count);

                while (m_buffer.BytesAvailable > 0)
                {
                    if (m_currentMessage == null)
                        m_currentMessage = new MessagePart();

                    // if message is complete
                    if (m_currentMessage.Build(m_buffer))
                    {
                        var messageDataReader = new BigEndianReader(m_currentMessage.Data);
                        NetworkMessage message = MessageBuilder.BuildMessage((uint)m_currentMessage.MessageId.Value, messageDataReader);

                        LastActivity = DateTime.Now;
                        OnMessageReceived(message);

                        m_currentMessage = null;
                    }
                }
            }
        }

        protected void Close()
        {
            if (Socket != null && Socket.Connected)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                Socket = null;
            }
        }

        public override string ToString()
        {
            return string.Concat("<", IP, ">");
        }
    }
}