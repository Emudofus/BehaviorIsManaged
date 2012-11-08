#region License GNU GPL
// BotManager.cs
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
            if (!bot.Disposed)
                bot.Dispose();

            if (m_bots.Remove(bot))
                OnBotRemoved(bot);
        }

        public Bot GetCurrentBot()
        {
            var bot = Bots.FirstOrDefault(entry => entry.IsInContext);

            return bot;
        }

        public void RemoveAll()
        {
            foreach (var bot in m_bots.ToArray())
            {
                RemoveBot(bot);
            }
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