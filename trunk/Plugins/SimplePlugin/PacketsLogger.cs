using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BiM.Behaviors;
using BiM.Behaviors.Messages;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Core.Reflection;
using BiM.Host.Messages;
using BiM.MITM;

namespace SimplePlugin
{
    public static class PacketsLogger
    {
        public static bool AllowLogging
        {
            get { return Config.GetStatic("AllowLogging", false); }
        }

        private static ObjectDumper m_dumper = new ObjectDumper(2, true, false, BindingFlags.Public | BindingFlags.Instance |
            BindingFlags.GetField | BindingFlags.FlattenHierarchy);

        [MessageHandler(typeof(BotCreatedMessage))]
        public static void Initialize(Bot bot, BotCreatedMessage message)
        {
            if (AllowLogging)
                bot.Dispatcher.MessageDispatched += OnMessageDispatched;
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