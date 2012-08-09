using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using BiM.Behaviors;
using BiM.Core.Extensions;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.MITM.Network;
using BiM.Protocol.Messages;
using BiM.Protocol.Types;
using NLog;

namespace BiM.MITM
{
    public class MITM
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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

            MessageBuilder = new MessageReceiver();
            MessageBuilder.Initialize();

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

            logger.Info("MITM started");
        }

        public void Stop()
        {
            AuthConnections.Stop();
            WorldConnections.Stop();

            logger.Info("MITM stoped");
        }

        private ConnectionMITM CreateAuthClient(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            var client = new ConnectionMITM(socket, MessageBuilder);
            client.MessageReceived += OnAuthClientMessageReceived;

            var dispatcher = new NetworkMessageDispatcher {Client = client, Server = client.Server};

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

            return client;
        }

        private void OnAuthClientConnected(ConnectionMITM client)
        {
            client.Bot.Start();
            client.BindToServer(m_configuration.RealAuthHost, m_configuration.RealAuthPort); 
            logger.Debug("Auth client connected");
        }

        private void OnAuthClientDisconnected(ConnectionMITM client)
        {
            client.Bot.Stop();
        }

        private void OnWorldClientConnected(ConnectionMITM client)
        {
            // todo : config
            client.Send(new ProtocolRequired(1459, 1459));
            client.Send(new HelloGameMessage());

            logger.Debug("World client connected");
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

            mitm.Bot.Dispatcher.Enqueue(message, mitm.Bot);

            logger.Debug("{0} FROM {1}", message, message.From);
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

                mitm.Bot.Dispatcher.Enqueue(message, mitm.Bot);
            }

            logger.Debug("{0} FROM {1}", message, message.From);
        }

        private void HandleAuthenticationTicketMessage(ConnectionMITM client, AuthenticationTicketMessage message)
        {
            if (!m_tickets.ContainsKey(message.ticket))
                throw new Exception(string.Format("Ticket {0} not registered", message.ticket));

            var tuple = m_tickets[message.ticket];

            m_tickets.Remove(message.ticket);

            client.Bot = tuple.Item1;
            client.Bot.ChangeConnection(client);
            client.Bot.ConnectionType = ClientConnectionType.GameConnection;
            client.Bot.Start();

            ( client.Bot.Dispatcher as NetworkMessageDispatcher ).Client = client;
            ( client.Bot.Dispatcher as NetworkMessageDispatcher ).Server = client.Server;
            client.BindToServer(tuple.Item2.address, tuple.Item2.port);

            logger.Debug("Bot retrieved with ticket {0}", message.ticket);
        }


        [MessageHandler(typeof(SelectedServerDataMessage), FromFilter = ListenerEntry.Server)]
        private void HandleSelectedServerDataMessage(Bot bot, SelectedServerDataMessage message)
        {
            m_tickets.Add(message.ticket, Tuple.Create((BotMITM)bot, new SelectedServerDataMessage(message.serverId, message.address, message.port, message.canCreateNewCharacter, message.ticket)));

            message.address = m_configuration.FakeWorldHost;
            message.port = (ushort) m_configuration.FakeWorldPort;

            logger.Debug("Client redirected to {0}:{1}", message.address, message.port);
        }

        [MessageHandler(typeof(AuthenticationTicketMessage), FromFilter = ListenerEntry.Client)]
        private static void HandleAuthenticationTicketMessage(Bot bot, AuthenticationTicketMessage message)
        {
            message.BlockNetworkSend();
        }

        [MessageHandler(typeof(ProtocolRequired), FromFilter = ListenerEntry.Server)]
        private void HandleProtocolRequired(Bot bot, ProtocolRequired message)
        {
            if (bot.ConnectionType == ClientConnectionType.GameConnection)
                message.BlockNetworkSend();
        }

        [MessageHandler(typeof(HelloGameMessage), FromFilter = ListenerEntry.Server)]
        private void HandleHelloGameMessage(Bot bot, HelloGameMessage message)
        {
            message.BlockNetworkSend();

            bot.SendToServer(new AuthenticationTicketMessage("fr", bot.ClientInformations.ConnectionTicket));
        }
    }
}