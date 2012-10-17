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