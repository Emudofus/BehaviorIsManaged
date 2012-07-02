using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using BiM.Core.Collections;
using BiM.Core.Extensions;
using Stump.Core.Timers;

namespace BiM.Core.Threading
{
    /// <summary>
    /// Thank's to WCell project
    /// </summary>
    public class SelfRunningTaskQueue
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly LockFreeQueue<IMessage> m_messageQueue;
        private readonly Stopwatch m_queueTimer;
        private readonly List<SimpleTimerEntry> m_simpleTimers;
        private readonly List<TimerEntry> m_timers;

        private int m_currentThreadId;
        private Task m_updateTask;
        private int m_lastUpdate;

        public SelfRunningTaskQueue(int interval, string name)
        {
            m_messageQueue = new LockFreeQueue<IMessage>();
            m_queueTimer = Stopwatch.StartNew();
            m_simpleTimers = new List<SimpleTimerEntry>();
            m_timers = new List<TimerEntry>();
            UpdateInterval = interval;
            Name = name;

            Start();
        }

        public string Name
        {
            get;
            set;
        }

        public int UpdateInterval
        {
            get;
            set;
        }

        public long LastUpdateTime
        {
            get { return m_lastUpdate; }
        }

        public bool IsRunning
        {
            get;
            protected set;
        }

        public void Start()
        {
            IsRunning = true;

            m_updateTask = Task.Factory.StartNewDelayed(UpdateInterval, ProcessCallback, this);
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public bool IsInContext
        {
            get
            {
                return Thread.CurrentThread.ManagedThreadId == m_currentThreadId;
            }
        }

        public void AddMessage(IMessage message)
        {
            m_messageQueue.Enqueue(message);
        }

        public void AddMessage(Action action)
        {
            m_messageQueue.Enqueue(new Message(action));
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

        public void AddTimer(TimerEntry timer)
        {
            AddMessage(() => m_timers.Add(timer));
        }

        public void RemoveTimer(TimerEntry timer)
        {
            AddMessage(() => m_timers.Remove(timer));
        }

        public void CancelSimpleTimer(SimpleTimerEntry timer)
        {
            m_simpleTimers.Remove(timer);
        }

        public SimpleTimerEntry CallPeriodically(int delayMillis, Action callback)
        {
            var timer = new SimpleTimerEntry(delayMillis, callback, LastUpdateTime, false);
            m_simpleTimers.Add(timer);
            return timer;
        }

        public SimpleTimerEntry CallDelayed(int delayMillis, Action callback)
        {
            var timer = new SimpleTimerEntry(delayMillis, callback, LastUpdateTime, true);
            m_simpleTimers.Add(timer);
            return timer;
        }

        internal int GetDelayUntilNextExecution(SimpleTimerEntry timer)
        {
            return timer.Delay - (int)( LastUpdateTime - timer.LastCallTime );
        }

        protected void ProcessCallback(object state)
        {
            try
            {
                if (!IsRunning)
                {
                    return;
                }

                if (Interlocked.CompareExchange(ref m_currentThreadId, Thread.CurrentThread.ManagedThreadId, 0) == 0)
                {
                    // get the time at the start of our task processing
                    var timerStart = m_queueTimer.ElapsedMilliseconds;
                    var updateDt = (int)( timerStart - m_lastUpdate );
                    m_lastUpdate = (int)timerStart;

                    // process timer entries
                    foreach (var timer in m_timers)
                    {
                        try
                        {
                            timer.Update(updateDt);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Failed to update {0} : {1}", timer, ex);
                        } 
                        
                        if (!IsRunning)
                        {
                            return;
                        }
                    }

                    // process timers
                    var count = m_simpleTimers.Count;
                    for (var i = count - 1; i >= 0; i--)
                    {
                        var timer = m_simpleTimers[i];
                        if (GetDelayUntilNextExecution(timer) <= 0)
                        {
                            try
                            {
                                timer.Execute(this);
                            }
                            catch (Exception ex)
                            {
                                logger.Error("Failed to execute timer {0} : {1}", timer, ex);
                            }
                        } 
                        
                        if (!IsRunning)
                        {
                            return;
                        }
                    }

                    // process messages
                    IMessage msg;
                    while (m_messageQueue.TryDequeue(out msg))
                    {
                        try
                        {
                            msg.Execute();
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Failed to execute message {0} : {1}", msg, ex);
                        }

                        if (!IsRunning)
                        {
                            return;
                        }
                    }
                    // get the end time
                    long timerStop = m_queueTimer.ElapsedMilliseconds;

                    bool updateLagged = timerStop - timerStart > UpdateInterval;
                    long callbackTimeout = updateLagged ? 0 : ( ( timerStart + UpdateInterval ) - timerStop );

                    Interlocked.Exchange(ref m_currentThreadId, 0);

                    if (updateLagged)
                        logger.Debug("TaskPool '{0}' update lagged ({1}ms)", Name, timerStop - timerStart);

                    if (IsRunning)
                    {
                        // re-register the Update-callback
                        m_updateTask = Task.Factory.StartNewDelayed((int)callbackTimeout, ProcessCallback, this);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to run TaskQueue callback for \"{0}\" : {1}", Name, ex);
            }
        }
    }
}