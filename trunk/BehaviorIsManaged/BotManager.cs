using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BiM.Core.Reflection;

namespace BiM
{
    public class BotManager : Singleton<BotManager>
    {
        public event Action<BotManager, Bot> BotAdded;

        private void OnBotAdded(Bot bot)
        {
            Action<BotManager, Bot> handler = BotAdded;
            if (handler != null) handler(this, bot);
        }

        public event Action<BotManager, Bot> BotRemoved;

        private void OnBotRemoved(Bot bot)
        {
            Action<BotManager, Bot> handler = BotRemoved;
            if (handler != null) handler(this, bot);
        }

        private readonly List<Bot> m_bots = new List<Bot>();

        public ReadOnlyCollection<Bot> Bots
        {
            get { return m_bots.AsReadOnly(); }
        }

        public void RegisterBot(Bot bot)
        {
            m_bots.Add(bot);

            OnBotAdded(bot);

            Task.Factory.StartNew(() => FakeBotTaskRunning(bot));
        }

        public void RemoveBot(Bot bot)
        {
            m_bots.Remove(bot);

            OnBotRemoved(bot);
        }

        private void FakeBotTaskRunning(Bot bot)
        {
            while (bot.Running)
            {
                bot.Tick();
            }
        }
    }
}