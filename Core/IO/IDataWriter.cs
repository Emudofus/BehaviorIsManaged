#region License GNU GPL
// IDataWriter.cs
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

namespace BiM.Core.IO
{
    public interface IDataWriter
    {
        byte[] Data
        {
            get;
        }

        /// <summary>
        ///   Write a Short into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteShort(short @short);

        /// <summary>
        ///   Write a int into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteInt(int @int);

        /// <summary>
        ///   Write a long into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteLong(Int64 @long);

        /// <summary>
        ///   Write a UShort into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteUShort(ushort @ushort);

        /// <summary>
        ///   Write a int into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteUInt(UInt32 @uint);

        /// <summary>
        ///   Write a long into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteULong(UInt64 @ulong);

        /// <summary>
        ///   Write a byte into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteByte(byte @byte);
        void WriteSByte(sbyte @byte);

        /// <summary>
        ///   Write a Float into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteFloat(float @float);

        /// <summary>
        ///   Write a Boolean into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteBoolean(Boolean @bool);

        /// <summary>
        ///   Write a Char into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteChar(Char @char);

        /// <summary>
        ///   Write a Double into the buffer
        /// </summary>
        void WriteDouble(Double @double);

        /// <summary>
        ///   Write a Single into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteSingle(Single @single);

        /// <summary>
        ///   Write a string into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteUTF(string str);

        /// <summary>
        ///   Write a string into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteUTFBytes(string str);

        /// <summary>
        ///   Write a bytes array into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteBytes(byte[] data);

        void Clear();
    }
}