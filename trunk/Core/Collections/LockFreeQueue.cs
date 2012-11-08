#region License GNU GPL
// LockFreeQueue.cs
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
using System.Threading;

/* LockFreeQueue copyright (c) julian m bucknall
 * http://www.boyet.com/Articles/LockfreeQueue.html
*/

namespace BiM.Core.Collections
{
   
    internal class SingleLinkNode<T>
    {
        public SingleLinkNode<T> Next;
        public T Item;
    }

    public class LockFreeQueue<T> : IEnumerable<T>
    {
        private SingleLinkNode<T> _head;
        private SingleLinkNode<T> _tail;
        private int _count;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LockFreeQueue()
        {
            _head = new SingleLinkNode<T>();
            _tail = _head;
        }

        public LockFreeQueue(IEnumerable<T> items)
            : this()
        {
            foreach (var item in items)
            {
                Enqueue(item);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the queue.
        /// </summary>
        public int Count
        {
            get { return Thread.VolatileRead(ref _count); }
        }

        /// <summary>
        /// Adds an object to the end of the queue.
        /// </summary>
        /// <param name="item">the object to add to the queue</param>
        public void Enqueue(T item)
        {
            SingleLinkNode<T> oldTail = null;
            SingleLinkNode<T> oldTailNext;

            var newNode = new SingleLinkNode<T> { Item = item };

            bool newNodeWasAdded = false;

            while (!newNodeWasAdded)
            {
                oldTail = _tail;
                oldTailNext = oldTail.Next;

                if (_tail == oldTail)
                {
                    if (oldTailNext == null)
                    {
                        newNodeWasAdded =
                            Interlocked.CompareExchange<SingleLinkNode<T>>(ref _tail.Next, newNode, null) == null;
                    }
                    else
                    {
                        Interlocked.CompareExchange<SingleLinkNode<T>>(ref _tail, oldTailNext, oldTail);
                    }
                }
            }

            Interlocked.CompareExchange<SingleLinkNode<T>>(ref _tail, newNode, oldTail);
            Interlocked.Increment(ref _count);
        }

        public T TryDequeue()
        {
            T item;
            TryDequeue(out item);
            return item;
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the queue.
        /// </summary>
        /// <param name="item">
        /// when the method returns, contains the object removed from the beginning of the queue, 
        /// if the queue is not empty; otherwise it is the default value for the element type
        /// </param>
        /// <returns>
        /// true if an object from removed from the beginning of the queue; 
        /// false if the queue is empty
        /// </returns>
        public bool TryDequeue(out T item)
        {
            item = default(T);
            SingleLinkNode<T> oldHead = null;

            bool haveAdvancedHead = false;
            while (!haveAdvancedHead)
            {
                oldHead = _head;
                SingleLinkNode<T> oldTail = _tail;
                SingleLinkNode<T> oldHeadNext = oldHead.Next;

                if (oldHead == _head)
                {
                    if (oldHead == oldTail)
                    {
                        if (oldHeadNext == null)
                            return false;

                        Interlocked.CompareExchange<SingleLinkNode<T>>(ref _tail, oldHeadNext, oldTail);
                    }

                    else
                    {
                        item = oldHeadNext.Item;
                        haveAdvancedHead =
                          Interlocked.CompareExchange<SingleLinkNode<T>>(ref _head, oldHeadNext, oldHead) == oldHead;
                    }
                }
            }

            Interlocked.Decrement(ref _count);
            return true;
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the queue.
        /// </summary>
        /// <returns>the object that is removed from the beginning of the queue</returns>
        public T Dequeue()
        {
            T result;

            if (!TryDequeue(out result))
            {
                throw new InvalidOperationException("the queue is empty");
            }

            return result;
        }

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the queue.
        /// </summary>
        /// <returns>an enumerator for the queue</returns>
        public IEnumerator<T> GetEnumerator()
        {
            SingleLinkNode<T> currentNode = _head;

            do
            {
                if (currentNode.Item == null)
                {
                    yield break;
                }
                else
                {
                    yield return currentNode.Item;
                }
            }
            while ((currentNode = currentNode.Next) != null);

            yield break;
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the queue.
        /// </summary>
        /// <returns>an enumerator for the queue</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Clears the queue.
        /// </summary>
        /// <remarks>This method is not thread-safe.</remarks>
        public void Clear()
        {
            SingleLinkNode<T> tempNode;
            SingleLinkNode<T> currentNode = _head;

            while (currentNode != null)
            {
                tempNode = currentNode;
                currentNode = currentNode.Next;

                tempNode.Item = default(T);
                tempNode.Next = null;
            }

            _head = new SingleLinkNode<T>();
            _tail = _head;
            _count = 0;
        }

    }
}
