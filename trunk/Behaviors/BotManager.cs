using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BiM.Core.Logging;
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
            var target = new MethodCallTarget()
            {
                Name = "botLog",
                ClassName = GetType().FullName + "," + GetType().Assembly.GetName().Name,
                MethodName = "LogNotified"
            };

            target.Parameters.Add(new MethodCallParameter("level", "${level}"));
            target.Parameters.Add(new MethodCallParameter("caller", "${callsite:className=true:methodName=true:includeSourcePath=false:nonamespace=true}"));
            target.Parameters.Add(new MethodCallParameter("message", "${message}"));

            NLogHelper.AddTarget(target);
            NLogHelper.AddLogRule(new LoggingRule("*", NLog.LogLevel.Debug, target));
        }

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
        }

        public void RemoveBot(Bot bot)
        {
            m_bots.Remove(bot);

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