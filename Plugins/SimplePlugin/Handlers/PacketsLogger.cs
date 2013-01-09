#region License GNU GPL
// PacketsLogger.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System.Diagnostics;
using System.IO;
using System.Reflection;
using BiM.Behaviors;
using BiM.Behaviors.Messages;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Core.Reflection;
using System;

namespace SimplePlugin.Handlers
{
    public static class PacketsLogger
    {
        [Configurable("AllowLogging", "If true, all messsages for this char will be dumped in a file")]
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
            try
            {
            File.AppendAllText(file, m_dumper.Dump(message) + "\r\n");
        }
            catch(Exception ex)
            {
                bot.Character.SendError("The log file for {0} can't be written in {1} : {2}", bot.Character, file, ex.Message);
                AllowLogging = false;
            }
        }
    }
}