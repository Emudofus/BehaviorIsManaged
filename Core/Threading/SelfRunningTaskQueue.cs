#region License GNU GPL
// SelfRunningTaskQueue.cs
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
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using BiM.Core.Collections;
using BiM.Core.Extensions;
using NLog;

namespace BiM.Core.Threading
{
    public class SelfRunningTaskQueue : INotifyPropertyChanged
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly LockFreeQueue<Action> m_messageQueue;
        private readonly Stopwatch m_queueTimer;
        private int m_currentThreadId;
        private int m_lastUpdate;
        private Task m_updateTask;
        private readonly List<SimplerTimer> m_timers;

        protected SelfRunningTaskQueue(int updateInterval)
        {
            m_timers = new List<SimplerTimer>();
            m_messageQueue = new LockFreeQueue<Action>();
            m_queueTimer = Stopwatch.StartNew();
            UpdateInterval = updateInterval;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public int UpdateInterval
        {
            get;
            set;
        }

        private bool m_running;

        public bool Running
        {
            get { return m_running; }
            protected set
            {
                m_running = value;
            }
        }


        public virtual void Start()
        {
           Running = true;

           m_updateTask = Task.Factory.StartNewDelayed(UpdateInterval, Tick);
        }

        public virtual void Stop()
        {
            Running = false;
        }

        public bool IsInContext
        {
            get
            {
                return Thread.CurrentThread.ManagedThreadId == m_currentThreadId;
            }
        }

        public void AddMessage(Action action)
        {
            m_messageQueue.Enqueue(action);
        }

        public void EnsureNotContext()
        {
            if (IsInContext)
            {
                throw new InvalidOperationException("Forbidden context");
            }
        }

        public void EnsureContext()
        {
            if (!IsInContext)
            {
                throw new InvalidOperationException("Not in context");
            }
        }

        public bool ExecuteInContext(Action action)
        {
            if (IsInContext)
            {
                action();
                return true;
            }

            AddMessage(action);
            return false;
        }

        public void AddTimer(SimplerTimer timer)
        {
            AddMessage(() => m_timers.Add(timer));
        }

        public void RemoveTimer(SimplerTimer timer)
        {
            AddMessage(() => m_timers.Remove(timer));
        }

        public SimplerTimer CallPeriodically(int delayMillis, Action callback)
        {
            var timer = new SimplerTimer(0, delayMillis, callback);
            timer.Start();
            AddTimer(timer);
            return timer;
        }

        public SimplerTimer CallDelayed(int delayMillis, Action callback)
        {
            var timer = new SimplerTimer(delayMillis, 0, callback);
            timer.Start();
            AddTimer(timer);
            return timer;
        }

        public void CancelAllMessages()
        {
            m_messageQueue.Clear();
        }

        protected virtual void OnTick()
        {
        }

        private void Tick()
        {
            if (!Running)
                return;

            try
            {
                if (Interlocked.CompareExchange(ref m_currentThreadId, Thread.CurrentThread.ManagedThreadId, 0) == 0)
                {
                    long timerStart = m_queueTimer.ElapsedMilliseconds;
                    var updateDt = (int)( timerStart - m_lastUpdate );
                    m_lastUpdate = (int)timerStart;

                    // do stuff here

                    // process timer entries
                    foreach (var timer in m_timers)
                    {
                        if (!timer.IsRunning)
                        {
                            RemoveTimer(timer);
                            continue;
                        }

                        try
                        {
                            timer.Update(updateDt);
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorException(string.Format("Failed to update {0}", timer), ex);
                        }

                        if (!timer.IsRunning)
                            RemoveTimer(timer);

                        if (!Running)
                        {
                            Interlocked.Exchange(ref m_currentThreadId, 0);
                            return;
                        }
                    }

                    Action msg;
                    while (m_messageQueue.TryDequeue(out msg))
                    {
                        try
                        {
                            msg();
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorException(string.Format("Failed to execute message {0}", msg), ex);
                        }

                        if (!Running)
                        {
                            Interlocked.Exchange(ref m_currentThreadId, 0);
                            return;
                        }
                    }

                    OnTick();

                    Interlocked.Exchange(ref m_currentThreadId, 0);

                    // get the end time
                    long timerStop = m_queueTimer.ElapsedMilliseconds;

                    bool updateLagged = timerStop - timerStart > UpdateInterval;
                    long callbackTimeout = updateLagged ? 0 : ( ( timerStart + UpdateInterval ) - timerStop );

                    Interlocked.Exchange(ref m_currentThreadId, 0);

                    if (updateLagged)
                        logger.Debug("TaskPool '{0}' update lagged ({1}ms)", Name, timerStop - timerStart);

                    if (Running)
                    {
                        // re-register the Update-callback
                        m_updateTask = Task.Factory.StartNewDelayed((int)callbackTimeout, Tick);
                    }
                }
                else Debug.WriteLine("");
            }
            catch (Exception ex)
            {
                logger.FatalException(string.Format("Failed to run TaskQueue callback for \"{0}\"", Name), ex);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void FirePropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}