#region License GNU GPL
// WeakReference.cs
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
using System.Runtime.InteropServices;

namespace BiM.Core.Memory
{
    /// <summary>
    /// Represents a weak reference, which references an object while still allowing that object to be reclaimed by garbage collection.
    /// </summary>
    /// <remarks>
    /// <para>We define our own type, unrelated to <see cref="System.WeakReference"/> both to provide type safety and because <see cref="System.WeakReference"/> is an incorrect implementation (it does not implement <see cref="IDisposable"/>).</para>
    /// </remarks>
    /// <typeparam name="T">The type of object to reference.</typeparam>
    public sealed class WeakReference<T> : IDisposable where T : class
    {
        /// <summary>
        /// The contained <see cref="SafeGCHandle"/>.
        /// </summary>
        private SafeGCHandle safeHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class, referencing the specified object.
        /// </summary>
        /// <param name="target">The object to track. May not be null.</param>
        public WeakReference(T target)
        {
            this.safeHandle = new SafeGCHandle(target, GCHandleType.Weak);
        }

        /// <summary>
        /// Gets the referenced object. Will return null if the object has been garbage collected.
        /// </summary>
        public T Target
        {
            get
            {
                return this.safeHandle.Handle.Target as T;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object is still alive (has not been garbage collected).
        /// </summary>
        public bool IsAlive
        {
            get
            {
                return this.safeHandle.Handle.Target != null;
            }
        }

        /// <summary>
        /// Frees the weak reference.
        /// </summary>
        public void Dispose()
        {
            this.safeHandle.Dispose();
        }
    }
}