#region License GNU GPL
// PriorityQueueB.cs
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
using System.Collections;
using System.Collections.Generic;

namespace BiM.Core.Collections
{
    public interface IPriorityQueue<T>
    {
        int Push(T item);
        T Pop();
        T Peek();
        void Update(int i);
    }

    public class PriorityQueueB<T> : IPriorityQueue<T>, IEnumerable<T>
    {
        protected IComparer<T> Comparer;
        protected List<T> InnerList = new List<T>();

        public PriorityQueueB()
        {
            Comparer = Comparer<T>.Default;
        }

        public PriorityQueueB(IComparer<T> comparer)
        {
            Comparer = comparer;
        }

        public PriorityQueueB(IComparer<T> comparer, int capacity)
        {
            Comparer = comparer;
            InnerList.Capacity = capacity;
        }

        public int Count
        {
            get { return InnerList.Count; }
        }

        public T this[int index]
        {
            get { return InnerList[index]; }
            set
            {
                InnerList[index] = value;
                Update(index);
            }
        }

        #region IPriorityQueue<T> Members

        /// <summary>
        /// Push an object onto the PQ
        /// </summary>
        /// <param name="item">The new object</param>
        /// <returns>The index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.</returns>
        public int Push(T item)
        {
            int p = InnerList.Count, p2;
            InnerList.Add(item); // E[p] = O
            do
            {
                if (p == 0)
                    break;
                p2 = (p - 1)/2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                    break;
            } while (true);
            return p;
        }

        /// <summary>
        /// Get the smallest object and remove it.
        /// </summary>
        /// <returns>The smallest object</returns>
        public T Pop()
        {
            T result = InnerList[0];
            int p = 0, p1, p2, pn;
            InnerList[0] = InnerList[InnerList.Count - 1];
            InnerList.RemoveAt(InnerList.Count - 1);
            do
            {
                pn = p;
                p1 = 2*p + 1;
                p2 = 2*p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0)
                    p = p1;
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0)
                    p = p2;

                if (p == pn)
                    break;
                SwitchElements(p, pn);
            } while (true);

            return result;
        }

        /// <summary>
        /// Notify the PQ that the object at position i has changed
        /// and the PQ needs to restore order.
        /// Since you dont have access to any indexes (except by using the
        /// explicit IList.this) you should not call this function without knowing exactly
        /// what you do.
        /// </summary>
        /// <param name="i">The index of the changed object.</param>
        public void Update(int i)
        {
            int p = i,pn;
            int p2, p1;
            do
            {
                if (p == 0)
                    break;
                p2 = (p - 1)/2;
                if (OnCompare(p, p2) < 0)
                {
                    SwitchElements(p, p2);
                    p = p2;
                }
                else
                    break;
            } while (true);
            if (p < i)
                return;
            do
            {
                pn = p;
                p1 = 2*p + 1;
                p2 = 2*p + 2;
                if (InnerList.Count > p1 && OnCompare(p, p1) > 0)
                    p = p1;
                if (InnerList.Count > p2 && OnCompare(p, p2) > 0)
                    p = p2;

                if (p == pn)
                    break;
                SwitchElements(p, pn);
            } while (true);
        }

        /// <summary>
        /// Get the smallest object without removing it.
        /// </summary>
        /// <returns>The smallest object</returns>
        public T Peek()
        {
            if (InnerList.Count > 0)
                return InnerList[0];
            return default(T);
        }

        #endregion

        protected void SwitchElements(int i, int j)
        {
            T h = InnerList[i];
            InnerList[i] = InnerList[j];
            InnerList[j] = h;
        }

        protected virtual int OnCompare(int i, int j)
        {
            return Comparer.Compare(InnerList[i], InnerList[j]);
        }

        public void Clear()
        {
            InnerList.Clear();
        }

        public void Remove(T item)
        {
            int index = -1;
            for (int i = 0; i < InnerList.Count; i++)
            {
                if (Comparer.Compare(InnerList[i], item) == 0)
                    index = i;
            }

            if (index != -1)
                InnerList.RemoveAt(index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}