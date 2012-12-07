#region License GNU GPL
// SafeGCHandle.cs
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
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace BiM.Core.Memory
{
    /// <summary>
    /// Helper class to help with managing <see cref="GCHandle"/> resources.
    /// </summary>
    /// <remarks>
    /// <para>The only reason this isn't <c>public</c> is to prevent misuse by end users.</para>
    /// <para>Note that this class can only be used to represent <see cref="GCHandle"/> objects that should be freed when garbage collected (or disposed). This class cannot be used in several interop situations, such as passing ownership of an object to a callback function.</para>
    /// </remarks>
    internal sealed class SafeGCHandle : SafeHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeGCHandle"/> class referring to the target in the given way.
        /// </summary>
        /// <param name="target">The object to reference.</param>
        /// <param name="type">The way to reference the object.</param>
        public SafeGCHandle(object target, GCHandleType type)
            : base(IntPtr.Zero, true)
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                // The StyleCop warning for this try block is incorrect; it is required to create a Constrained Execution Region
            }
            finally
            {
                this.SetHandle(GCHandle.ToIntPtr(GCHandle.Alloc(target, type)));
            }
        }

        /// <summary>
        /// Gets the underlying allocated garbage collection handle.
        /// </summary>
        public GCHandle Handle
        {
            get
            {
                return GCHandle.FromIntPtr(this.handle);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the handle value is invalid.
        /// </summary>
        public override bool IsInvalid
        {
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [PrePrepareMethod]
            get
            {
                return this.handle == IntPtr.Zero;
            }
        }

        /// <summary>
        /// Frees the garbage collection handle.
        /// </summary>
        /// <returns>Whether the handle was released successfully.</returns>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [PrePrepareMethod]
        protected override bool ReleaseHandle()
        {
            GCHandle.FromIntPtr(this.handle).Free();
            return true;
        }
    }
}