using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.MITM;
using BiM.Protocol.Messages;

namespace BiM.Host
{
    public class Program
    {
        static Dictionary<string, SelectedServerDataMessage> m_tickets = new Dictionary<string, SelectedServerDataMessage>();
        static string ticket;
        private static MessageReceiver messageReceiver;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) => Console.WriteLine(eventArgs.ExceptionObject);

            messageReceiver = new MessageReceiver();
            messageReceiver.Initialize();

            MessageIdFinder.RegisterAssembly(typeof(MessageReceiver).Assembly);

            var clientManager = new ClientManager<BotConnection>(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555),
                CreateClientAuth);


            var clientManager2 = new ClientManager<BotConnection>(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5556),
                CreateClientWorld);

            clientManager.ClientConnected += OnClientConnected;
            clientManager2.ClientConnected += OnClientConnected2;

            clientManager.Start();
            clientManager2.Start();

            Console.Read();
        }

        private static BotConnection CreateClientAuth(Socket socket)
        {
            var dispatcher = new NetworkMessageDispatcher();
            dispatcher.RegisterAssembly(typeof(Program).Assembly);

            var client = new BotConnection(socket, messageReceiver, new Bot(dispatcher));
            client.MessageReceived += OnClientMessageReceived;
            dispatcher.Client = client;

            client.BindToServer("213.248.126.180", 5555);
            dispatcher.Server = client.Server;

            return client;
        }

        private static BotConnection CreateClientWorld(Socket socket)
        {
            var dispatcher = new NetworkMessageDispatcher();
            dispatcher.RegisterAssembly(typeof(Program).Assembly);

            var client = new BotConnection(socket, messageReceiver, new Bot(dispatcher));
            client.MessageReceived += OnClientMessageReceived;
            dispatcher.Client = client;

            client.Disconnected += client1 => ( client1 as BotConnection ).Bot.Running = false;

            client.Send(new ProtocolRequired(1428, 1428));
            client.Send(new HelloGameMessage());

            return client;
        }

        private static void OnClientConnected(BotConnection client)
        {
            Task.Factory.StartNew(() => BotTask(client.Bot));
        }

        private static void OnClientConnected2(BotConnection client)
        {
            Task.Factory.StartNew(() => BotTask(client.Bot));
        }

        private static void BotTask(Bot bot)
        {
            while (bot.Running)
            {
                bot.Dispatcher.ProcessDispatching(bot);

                Thread.Sleep(10);
            }
        }

        private static void OnClientMessageReceived(Client client, NetworkMessage message)
        {
            ((BotConnection)client).Bot.Dispatcher.AddMessageToDispatch(message);

            if (message.From == ListenerEntry.Server)
                Console.WriteLine("RECV " + message);
            else
                Console.WriteLine("SEND " + message);
        }

        [MessageHandler(SelectedServerDataMessage.Id)]
        public static void HandleSelectedServerDataMessage(Bot bot, SelectedServerDataMessage message)
        {
            message.address = "localhost";
            message.port = 5556;

            m_tickets.Add(message.ticket, message);
            ticket = message.ticket;
            bot.Running = false;
        }

        [MessageHandler(AuthenticationTicketMessage.Id)]
        public static void HandleAuthenticationTicketMessage(Bot bot, AuthenticationTicketMessage message)
        {
            var msg = m_tickets[message.ticket];

            m_tickets.Remove(message.ticket);

            var client = ( bot.Dispatcher as NetworkMessageDispatcher ).Client as BotConnection;

            client.BindToServer(msg.address, msg.port);
            (bot.Dispatcher as NetworkMessageDispatcher).Server = client.Server;

            message.BlockProgression(false);
        }

        [MessageHandler(ProtocolRequired.Id)]
        public static void HandleProtocolRequired(Bot bot, ProtocolRequired message)
        {
            message.BlockProgression(false);
        }

        [MessageHandler(HelloGameMessage.Id)]
        public static void HandleHelloGameMessage(Bot bot, HelloGameMessage message)
        {
            message.BlockProgression(false);
            bot.SendToServer(new AuthenticationTicketMessage("fr", ticket));
        }
    }
}
