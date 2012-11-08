#region License GNU GPL
// SimplerTimer.cs
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

namespace BiM.Core.Threading
{
    public class SimplerTimer : IDisposable
    {
        private int m_millisSinceLastTick;
        private int m_remainingInitialDelayMillis;
        private int m_intervalMillis;
        private Action m_action;

        public SimplerTimer()
        {
        }

        /// <summary>
        /// Creates a new timer with the given start delay, interval, and callback.
        /// </summary>
        /// <param name="delay">the delay before firing initially</param>
        /// <param name="intervalMillis">the interval between firing</param>
        /// <param name="callback">the callback to fire</param>
        public SimplerTimer(int delay, int intervalMillis, Action callback)
        {
            m_millisSinceLastTick = -1;
            m_action = callback;
            m_remainingInitialDelayMillis = delay;
            m_intervalMillis = intervalMillis;
        }

        public SimplerTimer(Action callback)
            : this(0, 0, callback)
        {
        }

        /// <summary>
        /// The amount of time in milliseconds that elapsed between the last timer tick and the last update.
        /// </summary>
        public int MillisSinceLastTick
        {
            get
            {
                return m_millisSinceLastTick;
            }
        }

        public int RemainingTime
        {
            get { return m_remainingInitialDelayMillis; }
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            m_millisSinceLastTick = 0;
        }

        /// <summary>
        /// Starts the timer with the given delay.
        /// </summary>
        /// <param name="initialDelay">the delay before firing initially</param>
        public void Start(int initialDelay)
        {
            m_remainingInitialDelayMillis = initialDelay;
            m_millisSinceLastTick = 0;
        }

        /// <summary>
        /// Starts the time with the given delay and interval.
        /// </summary>
        /// <param name="initialDelay">the delay before firing initially</param>
        /// <param name="interval">the interval between firing</param>
        public void Start(int initialDelay, int interval)
        {
            m_remainingInitialDelayMillis = initialDelay;
            m_intervalMillis = interval;
            m_millisSinceLastTick = 0;
        }

        /// <summary>
        /// Whether or not the timer is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return m_millisSinceLastTick >= 0;
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            m_millisSinceLastTick = -1;
        }

        /// <summary>
        /// Updates the timer, firing the callback if enough time has elapsed.
        /// </summary>
        /// <param name="dtMillis">the time change since the last update</param>
        public void Update(int dtMillis)
        {
            // means this timer is not running.
            if (m_millisSinceLastTick == -1)
                return;

            if (m_remainingInitialDelayMillis > 0)
            {
                m_remainingInitialDelayMillis -= dtMillis;

                if (m_remainingInitialDelayMillis <= 0)
                {
                    if (m_intervalMillis == 0)
                    {
                        // we need to stop the timer if it's only
                        // supposed to fire once.
                        Stop();
                        m_action();
                    }
                    else
                    {
                        m_action();
                        m_millisSinceLastTick = 0;
                    }
                }
            }
            else
            {
                // update our idle time
                m_millisSinceLastTick += dtMillis;

                if (m_millisSinceLastTick >= m_intervalMillis)
                {
                    // time to tick
                    m_action();
                    if (m_millisSinceLastTick != -1)
                    {
                        m_millisSinceLastTick -= m_intervalMillis;
                    }
                }
            }
        }

        /// <summary>
        /// Stops and cleans up the timer.
        /// </summary>
        public void Dispose()
        {
            Stop();
            m_action = null;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(SimplerTimer)) return false;
            return Equals((SimplerTimer)obj);
        }

        public bool Equals(SimplerTimer obj)
        {
            // needs to be improved
            return obj.m_intervalMillis == m_intervalMillis && Equals(obj.m_action, m_action);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = m_intervalMillis * 397 ^ ( m_action != null ? m_action.GetHashCode() : 0 );
                return result;
            }
        }
    }
}