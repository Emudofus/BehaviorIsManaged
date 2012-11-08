#region License GNU GPL
// Frame.cs
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
using System.Collections.Generic;
using System.Linq;

namespace BiM.Behaviors.Frames
{
    public abstract class Frame<T> : IFrame
        where T : IFrame
    {
        private static List<T> m_frames = new List<T>();

        private Bot m_bot;

        public Frame(Bot bot)
        {
            m_bot = bot;
        }

        public Bot Bot
        {
            get { return m_bot; }
        }

        public virtual void OnAttached()
        {
            lock (m_frames)
            {
                m_frames.Add((T)(IFrame)this);
            }

            Bot.Dispatcher.RegisterNonShared(this);
        }

        public virtual void OnDetached()
        {
            lock (m_frames)
            {
                m_frames.Remove((T)(IFrame)this);
            }

            Bot.Dispatcher.UnRegisterNonShared(this);
        }

        public static T GetFrame()
        {
            var bot = BotManager.Instance.GetCurrentBot();

            return GetFrame(bot);
        }

        public static T GetFrame(Bot bot)
        {
            lock (m_frames)
            {
                return m_frames.FirstOrDefault(x => x.Bot == bot);
            }
        }
    }
}