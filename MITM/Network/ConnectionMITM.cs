#region License GNU GPL
// ConnectionMITM.cs
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
using System.Net.Sockets;
using BiM.Core.Config;
using BiM.Core.Network;
using BiM.Core.Threading;
using NLog;

namespace BiM.MITM.Network
{
    /// <summary>
    /// Represent a connection bind between the client and the server
    /// </summary>
    public class ConnectionMITM : Client
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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

        public SimplerTimer TimeOutTimer
        {
            get;
            set;
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
            logger.Debug("The server closed the connection");
            Disconnect();
        }

        private void OnServerConnected(ServerConnection server)
        {
            logger.Debug("Connection to the server opened");
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