using System.Diagnostics;
using System.IO;
using System.Reflection;
using BiM.Behaviors;
using BiM.Behaviors.Messages;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Core.Reflection;

namespace SimplePlugin.Handlers
{
    public static class PacketsLogger
    {
        [Configurable("AllowLogging")]
        public static bool AllowLogging = false;

        private static ObjectDumper m_dumper = new ObjectDumper(2, true, false, BindingFlags.Public | BindingFlags.Instance |
            BindingFlags.GetField | BindingFlags.FlattenHierarchy);


        [MessageHandler(typeof(BotAddedMessage))]
        public static void OnBotAdded(object sender, BotAddedMessage message)
        {
            if (AllowLogging)
                message.Bot.Dispatcher.MessageDispatched += OnMessageDispatched;
        }

        private static void OnMessageDispatched(MessageDispatcher dispatcher, Message message)
        {
            var bot = (Bot)dispatcher.CurrentProcessor;
            var file = string.Format("{0}/log {1} {2}.log", Plugin.CurrentPlugin.GetPluginDirectory(), bot, Process.GetCurrentProcess().StartTime.ToString("dd MMM HH-mm-ss"));

            // this is thread safe
            File.AppendAllText(file, m_dumper.Dump(message) + "\r\n");
        }
    }
}