using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using BiM.Behaviors.Authentification;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Settings;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Core.Threading;
using NLog;

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
        private bool m_disposed;

        public bool Disposed
        {
            get { return m_disposed; }
        }


        public event LogHandler LogNotified;

        public void NotifyMessageLog(LogLevel level, string caller, string message)
        {
            LogHandler handler = LogNotified;
            if (handler != null) handler(this, level, caller, message);
        }

        public delegate void CharacterSelectedHandler(Bot bot, PlayedCharacter character);
        public event CharacterSelectedHandler CharacterSelected;

        protected void OnCharacterSelected(PlayedCharacter character)
        {
            CharacterSelectedHandler handler = CharacterSelected;
            if (handler != null) handler(this, character);
        }

        private List<IFrame> m_frames = new List<IFrame>(); 

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
            Display = DisplayState.None;
        }

        public MessageDispatcher Dispatcher
        {
            get;
            private set;
        }

        public BotSettings Settings
        {
            get;
            private set;
        }

        public ClientConnectionType ConnectionType
        {
            get;
            set;
        }

        public DisplayState Display
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
            private set;
        }

        public ReadOnlyCollection<IFrame> Frames
        {
            get { return m_frames.AsReadOnly(); }
        }

        public override string Name
        {
            get { return ToString(); }
            set { }
        }

        public void SetPlayedCharacter(PlayedCharacter character)
        {
            if (m_disposed)
                throw new Exception("Bot instance is disposed");

            if (character == null) throw new ArgumentNullException("character");
            if (Character != null)
                throw new Exception("Character already selected");

            Character = character;
            OnCharacterSelected(character);
        }

        public void LoadSettings(string path)
        {
            if (Settings != null)
                throw new Exception("Settings already loaded");

            Settings = new BotSettings(path);
            Settings.Load();
        }

        public void SaveSettings()
        {
            if (Settings != null)
                Settings.Save();
        }

        protected override void OnTick()
        {
            try
            {
                Dispatcher.ProcessDispatching(this);

                // note : not the correct way for the moment
                if (Character != null && Character.Context != null)
                    Character.Context.Tick(0);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);

                Stop();
            }

            base.OnTick();
        }

        #region Frames
        public bool AddFrame(IFrame frame)
        {
            if (HasFrame(frame.GetType()))
                return false;

            m_frames.Add(frame);
            frame.OnAttached();

            return true;
        }

        public bool RemoveFrame(IFrame frame)
        {
            if (m_frames.Remove(frame))
            {
                frame.OnDetached();
                return true;
            }

            return false;
        }

        public bool RemoveFrame<T>()
        {
            return RemoveFrame(typeof(T));
        }

        public bool RemoveFrame(Type type)
        {
            var frames = m_frames.Where(entry => entry.GetType().IsAssignableFrom(type)).ToArray();

            if (frames.Length > 1)
            {
                logger.Warn("Found {0} frames of type {1} (1 or 0 expected)", frames.Length, type);
            }

            return frames.Select(RemoveFrame).All(x => x);
        }

        public bool HasFrame(Type type)
        {
            return m_frames.Any(x => x.GetType().IsAssignableFrom(type));
        }

        public bool HasFrame<T>()
        {
            return HasFrame(typeof(T));
        }

        public IFrame GetFrame(Type type)
        {
            var frames = m_frames.Where(entry => entry.GetType().IsAssignableFrom(type)).ToArray();

            if (frames.Length > 1)
            {
                logger.Warn("Found {0} frames of type {1} (1 or 0 expected)", frames.Length, type);
            }

            return frames.FirstOrDefault();
        }


        public T GetFrame<T>()
        {
            var frames = m_frames.OfType<T>().ToArray();

            if (frames.Length > 1)
            {
                logger.Warn("Found {0} frames of type {1} (1 or 0 expected)", frames.Length, typeof(T));
            }

            return frames.FirstOrDefault();
        }

        #endregion

    
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

            if (Disposed)
                logger.Error("Error the message {0} wont be dispatched because the bot {1} is disposed !", message, this);
            else if (!Running)
                logger.Warn("Warning, enqueue {0} but the bot is stopped, the message will be processed once the bot {1} restart", message, this);
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

        public void SendLocal(Message message)
        {
            if (message == null) throw new ArgumentNullException("message");

            Dispatcher.Enqueue(message, this);

            if (Disposed)
                logger.Error("Error the message {0} wont be dispatched because the bot {1} is disposed !", message, this);
            else if (!Running)
                logger.Warn("Warning, enqueue {0} but the bot is stopped, the message will be processed once the bot {1} restart", message, this);
        }


        public override void Start()
        {
            if (Running)
                return;

            if (m_disposed)
                throw new Exception("Cannot start a disposed bot instance");

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
            if (m_disposed)
                return;

            m_disposed = true;

            Stop(); 
            
            foreach (var frame in m_frames)
            {
                frame.OnDetached();
            }

            m_frames.Clear();

            if (Dispatcher != null)
                Dispatcher.Dispose();

            BotManager.Instance.RemoveBot(this);

            logger.Debug("Bot removed");
        }

        public override string ToString()
        {
            if (Character != null && !string.IsNullOrEmpty(Character.Name))
                return Character.Name;

            if (ClientInformations != null && !string.IsNullOrEmpty(ClientInformations.Login))
                return ClientInformations.Login;

            return "Bot";
        }
    }
}