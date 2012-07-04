using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using BiM.Core.Extensions;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;

namespace BiM.MITM
{
    public class MITM
    {
        private readonly MITMConfiguration m_configuration;
        private readonly Dictionary<string, Tuple<BotMITM, SelectedServerDataMessage>> m_tickets = new Dictionary<string, Tuple<BotMITM, SelectedServerDataMessage>>();

        public MITM(MITMConfiguration configuration)
        {
            m_configuration = configuration;
            AuthConnections = new ClientManager<ConnectionMITM>(DnsExtensions.GetIPEndPointFromHostName(m_configuration.FakeAuthHost, m_configuration.FakeAuthPort), CreateAuthClient);
            WorldConnections = new ClientManager<ConnectionMITM>(DnsExtensions.GetIPEndPointFromHostName(m_configuration.FakeWorldHost, m_configuration.FakeWorldPort), CreateWorldClient);

            AuthConnections.ClientConnected += OnAuthClientConnected;
            AuthConnections.ClientDisconnected += OnAuthClientDisconnected;
            WorldConnections.ClientConnected += OnWorldClientConnected;
            WorldConnections.ClientDisconnected += OnWorldClientDisconnected;

            // todo : initialization somewhere else ?
            MessageBuilder = new MessageReceiver();
            MessageBuilder.Initialize();
            ProtocolTypeManager.Initialize();

            NetworkMessageDispatcher.RegisterContainer(this);
        }


        public MessageReceiver MessageBuilder
        {
            get;
            set;
        }

        public ClientManager<ConnectionMITM> AuthConnections
        {
            get;
            private set;
        }

        public ClientManager<ConnectionMITM> WorldConnections
        {
            get;
            private set;
        }

        public void Start()
        {
            AuthConnections.Start();
            WorldConnections.Start();
        }

        public void Stop()
        {
            AuthConnections.Stop();
            WorldConnections.Stop();
        }

        private ConnectionMITM CreateAuthClient(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            var client = new ConnectionMITM(socket, MessageBuilder);
            client.MessageReceived += OnAuthClientMessageReceived;
            client.MessageSent += OnAuthClientMessageSent;

            var dispatcher = new NetworkMessageDispatcher();
            dispatcher.Client = client;
            dispatcher.Server = client.Server;

            var bot = new BotMITM(client, dispatcher);
            client.Bot = bot;
            bot.ConnectionType = ClientConnectionType.Authentification;

            BotManager.Instance.RegisterBot(bot);


            return client;
        }

        private ConnectionMITM CreateWorldClient(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            var client = new ConnectionMITM(socket, MessageBuilder);
            client.MessageReceived += OnWorldClientMessageReceived;
            client.MessageSent += OnWorldClientMessageSent;

            return client;
        }

        private void OnAuthClientConnected(ConnectionMITM client)
        {
            client.BindToServer(m_configuration.RealAuthHost, m_configuration.RealAuthPort); 
            Console.WriteLine("Auth client connected");
        }

        private void OnAuthClientDisconnected(ConnectionMITM client)
        {
        }

        private void OnWorldClientConnected(ConnectionMITM client)
        {
            // todo : config
            client.Send(new ProtocolRequired(1459, 1459));
            client.Send(new HelloGameMessage());

            Console.WriteLine("World client connected");
        }

        private void OnWorldClientDisconnected(ConnectionMITM client)
        {
            client.Bot.Stop();
        }

        private void OnAuthClientMessageReceived(Client client, NetworkMessage message)
        {
            if (!( client is ConnectionMITM ))
                throw new ArgumentException("client is not of type ConnectionMITM");

            var mitm = client as ConnectionMITM;

            if (mitm.Bot == null)
                throw new NullReferenceException("mitm.Bot");

            mitm.Bot.Dispatcher.Enqueue(message);

            Console.WriteLine("{0} FROM {1}", message, message.From);
        }

        private void OnWorldClientMessageReceived(Client client, NetworkMessage message)
        {
            if (!( client is ConnectionMITM ))
                throw new ArgumentException("client is not of type ConnectionMITM");

            var mitm = client as ConnectionMITM;

            if (message is AuthenticationTicketMessage && mitm.Bot == null)
            {
                // special handling to connect and retrieve the bot instance
                HandleAuthenticationTicketMessage(mitm, message as AuthenticationTicketMessage);
            }
            else
            {
                if (mitm.Bot == null)
                    throw new NullReferenceException("mitm.Bot");

                mitm.Bot.Dispatcher.Enqueue(message);
            }

            Console.WriteLine("{0} FROM {1}", message, message.From);
        }

        private void OnAuthClientMessageSent(Client client, NetworkMessage message)
        {
        }

        private void OnWorldClientMessageSent(Client client, NetworkMessage message)
        {
        }

        private void HandleAuthenticationTicketMessage(ConnectionMITM client, AuthenticationTicketMessage message)
        {
            if (!m_tickets.ContainsKey(message.ticket))
                throw new Exception(string.Format("Ticket {0} not registered", message.ticket));

            var tuple = m_tickets[message.ticket];

            m_tickets.Remove(message.ticket);

            client.Bot = tuple.Item1;
            client.Bot.ConnectionType = ClientConnectionType.GameConnection;

            ( client.Bot.Dispatcher as NetworkMessageDispatcher ).Client = client;
            ( client.Bot.Dispatcher as NetworkMessageDispatcher ).Server = client.Server;
            client.BindToServer(tuple.Item2.address, tuple.Item2.port);


            Console.WriteLine("Retrive bot with ticket {0}", message.ticket);
        }


        [MessageHandler(SelectedServerDataMessage.Id, FromFilter = ListenerEntry.Server)]
        public void HandleSelectedServerDataMessage(Bot bot, SelectedServerDataMessage message)
        {
            bot.ConnectionTicket = message.ticket;
            m_tickets.Add(message.ticket, Tuple.Create((BotMITM)bot, new SelectedServerDataMessage(message.serverId, message.address, message.port, message.canCreateNewCharacter, message.ticket)));

            message.address = m_configuration.FakeWorldHost;
            message.port = (ushort) m_configuration.FakeWorldPort;
        }

        [MessageHandler(AuthenticationTicketMessage.Id, FromFilter = ListenerEntry.Client)]
        public static void HandleAuthenticationTicketMessage(Bot bot, AuthenticationTicketMessage message)
        {
            message.BlockNetworkSend();
        }

        [MessageHandler(ProtocolRequired.Id, FromFilter = ListenerEntry.Server)]
        public void HandleProtocolRequired(Bot bot, ProtocolRequired message)
        {
            if (bot.ConnectionType == ClientConnectionType.GameConnection)
                message.BlockNetworkSend();
        }

        [MessageHandler(HelloGameMessage.Id, FromFilter = ListenerEntry.Server)]
        public void HandleHelloGameMessage(Bot bot, HelloGameMessage message)
        {
            message.BlockNetworkSend();

            bot.SendToServer(new AuthenticationTicketMessage("fr", bot.ConnectionTicket));
        }
    }
}