using System;
using System.Net.Sockets;
using BiM.Core.Network;

namespace BiM.MITM.Network
{
    /// <summary>
    /// Represent a connection bind between the client and the server
    /// </summary>
    public class ConnectionMITM : Client
    {
        public ConnectionMITM(Socket clientSocket, IMessageBuilder messageBuilder)
            : base(clientSocket)
        {
            m_messageBuilder = messageBuilder;
            Server = new ServerConnection(messageBuilder);
        }

        private IMessageBuilder m_messageBuilder;

        public override IMessageBuilder MessageBuilder
        {
            get { return m_messageBuilder; }
        }

        public BotMITM Bot
        {
            get;
            set;
        }

        public bool IsBoundToServer
        {
            get { return Server != null && Server.IsConnected; }
        }

        public ServerConnection Server
        {
            get;
            private set;
        }

        /// <summary>
        /// Open a new connection and bind the client to the server
        /// </summary>
        public void BindToServer(string host, int port)
        {
            if (IsBoundToServer)
                throw new InvalidOperationException("Client already bound to server");

            Server.Connected += OnServerConnected;
            Server.Disconnected += OnServerDisconnected;
            Server.MessageReceived += OnServerMessageReceived;

            Server.Connect(host, port);
        }

        private void OnServerMessageReceived(ServerConnection server, NetworkMessage message)
        {
            message.From = ListenerEntry.Server;
            message.Destinations = ListenerEntry.Client | ListenerEntry.Local;

            base.OnMessageReceived(message);
        }

        private void OnServerDisconnected(ServerConnection server)
        {
            Disconnect();
        }

        private void OnServerConnected(ServerConnection server)
        {
        }

        protected override void OnMessageReceived(NetworkMessage message)
        {
            message.From = ListenerEntry.Client;
            message.Destinations = ListenerEntry.Server | ListenerEntry.Local;

            base.OnMessageReceived(message);
        }

        public void SendToServer(NetworkMessage message)
        {
            if (!IsBoundToServer)
                throw new InvalidOperationException("Client is not bound to server");

            Server.Send(message);
        }

        public override void Send(NetworkMessage message)
        {
            if (message.Destinations.HasFlag(ListenerEntry.Server))
                SendToServer(message);
            if (message.Destinations.HasFlag(ListenerEntry.Client))
                base.Send(message);
            else if (message.Destinations == ListenerEntry.Undefined)
                base.Send(message);
        }

        public override void Disconnect()
        {
            if (IsBoundToServer)
                Server.Disconnect();

            base.Disconnect();
        }
    }
}