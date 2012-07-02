using System;
using System.Threading;
using System.Threading.Tasks;

namespace BiM.Core.Extensions
{
    public static class TaskSchedulerExtensions
    {
        /// <summary>
        ///   Gets a SynchronizationContext that targets this TaskScheduler.
        /// </summary>
        /// <param name = "scheduler">The target scheduler.</param>
        /// <returns>A SynchronizationContext that targets this scheduler.</returns>
        public static SynchronizationContext ToSynchronizationContext(this TaskScheduler scheduler)
        {
            return new TaskSchedulerSynchronizationContext(scheduler);
        }

        #region Nested type: TaskSchedulerSynchronizationContext

        /// <summary>
        ///   Provides a SynchronizationContext wrapper for a TaskScheduler.
        /// </summary>
        private sealed class TaskSchedulerSynchronizationContext : SynchronizationContext
        {
            /// <summary>
            ///   The scheduler.
            /// </summary>
            private readonly TaskScheduler m_scheduler;

            /// <summary>
            ///   Initializes the context with the specified scheduler.
            /// </summary>
            /// <param name = "scheduler">The scheduler to target.</param>
            internal TaskSchedulerSynchronizationContext(TaskScheduler scheduler)
            {
                if (scheduler == null) throw new ArgumentNullException("scheduler");
                m_scheduler = scheduler;
            }

            /// <summary>
            ///   Dispatches an asynchronous message to the synchronization context.
            /// </summary>
            /// <param name = "d">The System.Threading.SendOrPostCallback delegate to call.</param>
            /// <param name = "state">The object passed to the delegate.</param>
            public override void Post(SendOrPostCallback d, object state)
            {
                Task.Factory.StartNew(() => d(state), CancellationToken.None, TaskCreationOptions.None, m_scheduler);
            }

            /// <summary>
            ///   Dispatches a synchronous message to the synchronization context.
            /// </summary>
            /// <param name = "d">The System.Threading.SendOrPostCallback delegate to call.</param>
            /// <param name = "state">The object passed to the delegate.</param>
            public override void Send(SendOrPostCallback d, object state)
            {
                var t = new Task(() => d(state));
                t.RunSynchronously(m_scheduler);
                t.Wait();
            }
        }

        #endregion
    }
}