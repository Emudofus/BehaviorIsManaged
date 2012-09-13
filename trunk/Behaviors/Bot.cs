using System;
using BiM.Behaviors.Authentification;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Messages;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Core.Threading;
using NLog;
using BiM.Behaviors.Managers;

namespace BiM.Behaviors
{
    public class Bot : SelfRunningTaskQueue
    {
        [Configurable("DefaultBotTick", "The interval (ms) between two message dispatching")]
        public static int DefaultBotTick = 100;

        #region Delegates

        public delegate void LogHandler(Bot bot, LogLevel level, string caller, string message);

        #endregion

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


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
            : base(DefaultBotTick)
        {
            if (messageDispatcher == null) throw new ArgumentNullException("messageDispatcher");
            Dispatcher = messageDispatcher;
            ConnectionType = ClientConnectionType.Disconnected;
            ClientInformations = new ClientInformations();
            ChatManager = new Managers.ChatManager(this);

            messageDispatcher.Enqueue(new BotCreatedMessage(), this);
        }

        public MessageDispatcher Dispatcher
        {
            get;
            private set;
        }

        public ChatManager ChatManager
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

        public override string Name
        {
            get { return ToString(); }
            set { }
        }

        protected override void OnTick()
        {
            try
            {
                Dispatcher.ProcessDispatching(this);

                // note : not the correct way for the moment
                if (Character != null && Character.Map != null)
                    Character.Map.Tick(0);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);

                Dispose();
            }

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

            BotManager.Instance.RemoveBot(this);

            logger.Debug("Bot removed");
        }

        public override string ToString()
        {
            return "Bot";
        }
    }
}