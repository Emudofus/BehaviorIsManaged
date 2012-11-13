#region License GNU GPL
// TaskCompletionSourceExtensions.cs
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
using System.Threading.Tasks;

namespace BiM.Core.Extensions
{
    /// <summary>
    ///   Extension methods for TaskCompletionSource.
    /// </summary>
    public static class TaskCompletionSourceExtensions
    {
        /// <summary>
        ///   Transfers the result of a Task to the TaskCompletionSource.
        /// </summary>
        /// <typeparam name = "TResult">Specifies the type of the result.</typeparam>
        /// <param name = "resultSetter">The TaskCompletionSource.</param>
        /// <param name = "task">The task whose completion results should be transfered.</param>
        public static void SetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task task)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    resultSetter.SetResult(task is Task<TResult> ? ((Task<TResult>) task).Result : default(TResult));
                    break;
                case TaskStatus.Faulted:
                    resultSetter.SetException(task.Exception.InnerExceptions);
                    break;
                case TaskStatus.Canceled:
                    resultSetter.SetCanceled();
                    break;
                default:
                    throw new InvalidOperationException("The task was not completed.");
            }
        }

        /// <summary>
        ///   Transfers the result of a Task to the TaskCompletionSource.
        /// </summary>
        /// <typeparam name = "TResult">Specifies the type of the result.</typeparam>
        /// <param name = "resultSetter">The TaskCompletionSource.</param>
        /// <param name = "task">The task whose completion results should be transfered.</param>
        public static void SetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task<TResult> task)
        {
            SetFromTask(resultSetter, (Task) task);
        }

        /// <summary>
        ///   Attempts to transfer the result of a Task to the TaskCompletionSource.
        /// </summary>
        /// <typeparam name = "TResult">Specifies the type of the result.</typeparam>
        /// <param name = "resultSetter">The TaskCompletionSource.</param>
        /// <param name = "task">The task whose completion results should be transfered.</param>
        /// <returns>Whether the transfer could be completed.</returns>
        public static bool TrySetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task task)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion:
                    return
                        resultSetter.TrySetResult(task is Task<TResult>
                                                      ? ((Task<TResult>) task).Result
                                                      : default(TResult));
                case TaskStatus.Faulted:
                    return resultSetter.TrySetException(task.Exception.InnerExceptions);
                case TaskStatus.Canceled:
                    return resultSetter.TrySetCanceled();
                default:
                    throw new InvalidOperationException("The task was not completed.");
            }
        }

        /// <summary>
        ///   Attempts to transfer the result of a Task to the TaskCompletionSource.
        /// </summary>
        /// <typeparam name = "TResult">Specifies the type of the result.</typeparam>
        /// <param name = "resultSetter">The TaskCompletionSource.</param>
        /// <param name = "task">The task whose completion results should be transfered.</param>
        /// <returns>Whether the transfer could be completed.</returns>
        public static bool TrySetFromTask<TResult>(this TaskCompletionSource<TResult> resultSetter, Task<TResult> task)
        {
            return TrySetFromTask(resultSetter, (Task) task);
        }
    }
}