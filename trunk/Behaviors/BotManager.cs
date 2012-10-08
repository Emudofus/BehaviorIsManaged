using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BiM.Behaviors.Messages;
using BiM.Core.Messages;
using BiM.Core.Reflection;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace BiM.Behaviors
{
    public class BotManager : Singleton<BotManager>
    {
        public void Initialize()
        {
            InterBotDispatcher = new MessageDispatcher();
            DispatcherTask = new DispatcherTask(InterBotDispatcher, this);
            DispatcherTask.Start();
        }

        public event Action<BotManager, Bot> BotAdded;

        private void OnBotAdded(Bot bot)
        {
            Action<BotManager, Bot> handler = BotAdded;
            if (handler != null) handler(this, bot);

            InterBotDispatcher.Enqueue(new BotAddedMessage(bot));
        }

        public event Action<BotManager, Bot> BotRemoved;

        private void OnBotRemoved(Bot bot)
        {
            Action<BotManager, Bot> handler = BotRemoved;
            if (handler != null) handler(this, bot);

            InterBotDispatcher.Enqueue(new BotRemovedMessage(bot));
        }

        private readonly List<Bot> m_bots = new List<Bot>();

        public ReadOnlyCollection<Bot> Bots
        {
            get { return m_bots.AsReadOnly(); }
        }

        public DispatcherTask DispatcherTask
        {
            get;
            private set;
        }

        public MessageDispatcher InterBotDispatcher
        {
            get;
            private set;
        }

        public void RegisterBot(Bot bot)
        {
            m_bots.Add(bot);

            OnBotAdded(bot);
        }

        public void RemoveBot(Bot bot)
        {
            if (m_bots.Remove(bot))
                OnBotRemoved(bot);
        }

        public Bot GetCurrentBot()
        {
            var bot = Bots.FirstOrDefault(entry => entry.IsInContext);

            return bot;
        }

        // have to be static
        public static void LogNotified(string level, string caller, string message)
        {
            var bot = Instance.GetCurrentBot();

            if (bot != null)
                bot.NotifyMessageLog(NLog.LogLevel.FromString(level), caller, message);
        }
    }
}