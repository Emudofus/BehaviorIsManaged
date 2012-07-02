using System;
using BiM.Core.Threading;

namespace Stump.Core.Timers
{
    public class SimpleTimerEntry
    {
        /// <summary>
        /// Whether this is a one-shot timer
        /// </summary>
        public readonly bool IsOneShot;

        internal SimpleTimerEntry(int delayMillis, Action callback, long time, bool isOneShot)
        {
            Callback = callback;
            Delay = delayMillis;
            LastCallTime = time;
            IsOneShot = isOneShot;
        }

        public long LastCallTime
        {
            get;
            private set;
        }

        public Action Callback
        {
            get;
            set;
        }

        public int Delay
        {
            get;
            set;
        }

        internal void Execute(SelfRunningTaskQueue queue)
        {
            Callback();
            LastCallTime = queue.LastUpdateTime;
            if (IsOneShot)
            {
                queue.CancelSimpleTimer(this);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is SimpleTimerEntry && Callback == ( (SimpleTimerEntry)obj ).Callback;
        }

        public override int GetHashCode()
        {
            return Callback.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} (Callback = {1}, Delay = {2})", GetType(), Callback, Delay);
        }
    }
}