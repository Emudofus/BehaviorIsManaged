using System;
using BiM.Behaviors.Authentification;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Core.Threading;
using NLog;

namespace BiM.Behaviors
{
    public class Bot : SelfRunningTaskQueue
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public delegate void LogHandler(Bot bot, LogLevel level, string caller, string message);
        public event LogHandler LogNotified;

        public void NotifyMessageLog(LogLevel level, string caller, string message)
        {
            LogHandler handler = LogNotified;
            if (handler != null) handler(this, level, caller, message);
        }

        public Bot()
            : this(new MessageDispatcher())
        {
        }

        public Bot(MessageDispatcher messageDispatcher)
            : base(30)
        {
            if (messageDispatcher == null) throw new ArgumentNullException("messageDispatcher");
            Dispatcher = messageDispatcher;
            ConnectionType = ClientConnectionType.Disconnected;
            ClientInformations = new ClientInformations();
        }

        public MessageDispatcher Dispatcher 
        { 
            get; 
            private set; 
        }

        public ClientConnectionType ConnectionType
        {
            get;
            set;
        }

        public ClientInformations ClientInformations
        {
            get;
            set;
        }

        public PlayedCharacter Character
        {
            get;
            set;
        }

        protected override void OnTick()
        {
            Dispatcher.ProcessDispatching(this);

            base.OnTick();
        }

        public void Send(Message message)
        {
            if (message == null) throw new ArgumentNullException("message");
            Dispatcher.Enqueue(message, this);
        }

        public void Send(NetworkMessage message, ListenerEntry dest)
        {
            if (message == null) throw new ArgumentNullException("message");
            message.Destinations = dest;
            message.From = ListenerEntry.Local;

            Dispatcher.Enqueue(message, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="direct">If true it doesn't reprocess the message internally</param>
        public void SendToClient(NetworkMessage message, bool direct = false)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (direct)
                Send(message, ListenerEntry.Client);
            else
                Send(message, ListenerEntry.Client | ListenerEntry.Local);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="direct">If true it doesn't reprocess the message internally</param>
        public void SendToServer(NetworkMessage message, bool direct = false)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (direct)
                Send(message, ListenerEntry.Server);
            else
                Send(message, ListenerEntry.Server | ListenerEntry.Local);
        }

        public void SendLocal(NetworkMessage message)
        {
            Send(message, ListenerEntry.Local);
        }

        public override void Start()
        {
            if (Running)
                return;

            if (Dispatcher.Stopped)
                Dispatcher.Resume();

            base.Start();

            logger.Debug("Bot started");
        }

        public override void Stop()
        {
            if (!Running)
                return;

            if (Dispatcher != null)
                Dispatcher.Stop();

            base.Stop();

            logger.Debug("Bot stopped");
        }

        public virtual void Dispose()
        {
            Stop();

            if (Dispatcher != null)
                Dispatcher.Dispose();
        }
    }
}