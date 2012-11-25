#region License GNU GPL
// IDataReader.cs
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
using System.IO;

namespace BiM.Core.IO
{
    public interface IDataReader : IDisposable
    {
        long Position
        {
            get;
        }

        long BytesAvailable
        {
            get;
        }

        /// <summary>
        ///   Read a Short from the Buffer
        /// </summary>
        /// <returns></returns>
        short ReadShort();

        /// <summary>
        ///   Read a int from the Buffer
        /// </summary>
        /// <returns></returns>
        int ReadInt();

        /// <summary>
        ///   Read a long from the Buffer
        /// </summary>
        /// <returns></returns>
        Int64 ReadLong();


        /// <summary>
        ///   Read a UShort from the Buffer
        /// </summary>
        /// <returns></returns>
        ushort ReadUShort();

        /// <summary>
        ///   Read a int from the Buffer
        /// </summary>
        /// <returns></returns>
        UInt32 ReadUInt();

        /// <summary>
        ///   Read a long from the Buffer
        /// </summary>
        /// <returns></returns>
        UInt64 ReadULong();

        /// <summary>
        ///   Read a byte from the Buffer
        /// </summary>
        /// <returns></returns>
        byte ReadByte();
        sbyte ReadSByte();

        /// <summary>
        ///   Returns N bytes from the buffer
        /// </summary>
        /// <param name = "n">Number of read bytes.</param>
        /// <returns></returns>
        byte[] ReadBytes(int n);

        /// <summary>
        ///   Read a Boolean from the Buffer
        /// </summary>
        /// <returns></returns>
        Boolean ReadBoolean();

        /// <summary>
        ///   Read a Char from the Buffer
        /// </summary>
        /// <returns></returns>
        Char ReadChar();

        /// <summary>
        ///   Read a Double from the Buffer
        /// </summary>
        /// <returns></returns>
        Double ReadDouble();

        /// <summary>
        ///   Read a Float from the Buffer
        /// </summary>
        /// <returns></returns>
        float ReadFloat();

        /// <summary>
        ///   Read a string from the Buffer
        /// </summary>
        /// <returns></returns>
        string ReadUTF();

        /// <summary>
        ///   Read a string from the Buffer
        /// </summary>
        /// <returns></returns>
        string ReadUTFBytes(ushort len);
        void Seek(int offset, SeekOrigin seekOrigin);
        void SkipBytes(int n);
    }
}