#region License GNU GPL
// FastLittleEndianReader.cs
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
using System.Text;

namespace BiM.Core.IO
{
    public unsafe class FastLittleEndianReader
    {
        private long m_position = 0;
        private readonly byte[] m_buffer;

        public byte[] Buffer
        {
            get { return m_buffer; }
        }

        public FastLittleEndianReader(byte[] buffer)
        {
            m_buffer = buffer;
        }

        public byte ReadByte()
        {
            fixed (byte* pbyte = &m_buffer[m_position++])
            {
                return *pbyte;
            }
        }

        public sbyte ReadSByte()
        {
            fixed (byte* pbyte = &m_buffer[m_position++])
            {
                return (sbyte)*pbyte;
            }
        }

        public long Position
        {
            get { return m_position; }
        }

        public long BytesAvailable
        {
            get { return m_buffer.Length - m_position; }
        }

        public short ReadShort()
        {
            var position = m_position;
            m_position += 2;
            fixed (byte* pbyte = &m_buffer[position])
            {
                    return (short)((*pbyte) | (*(pbyte + 1) << 8)) ; 
            }
        }

        public int ReadInt()
        {
            var position = m_position;
            m_position += 4;
            fixed (byte* pbyte = &m_buffer[position])
            {
                return ( *pbyte ) | ( *( pbyte + 1 ) << 8 ) | ( *( pbyte + 2 ) << 16 ) | ( *( pbyte + 3 ) << 24 );
            }
        }

        public long ReadLong()
        {
            var position = m_position;
            m_position += 8;
            fixed (byte* pbyte = &m_buffer[position])
            {
                int i1 = ( *pbyte ) | ( *( pbyte + 1 ) << 8 ) | ( *( pbyte + 2 ) << 16 ) | ( *( pbyte + 3 ) << 24 );
                int i2  = ( *( pbyte + 4 ) ) | ( *( pbyte + 5 ) << 8 ) | ( *( pbyte + 6 ) << 16 ) | ( *( pbyte + 7 ) << 24 );
                return (uint)i1 | ( (long)i2 << 32 ); 
            }
        }

        public ushort ReadUShort()
        {
            return (ushort)ReadShort();
        }

        public uint ReadUInt()
        {
            return (uint)ReadInt();
        }

        public ulong ReadULong()
        {
            return (ulong)ReadLong();
        }

        public byte[] ReadBytes(int n)
        {
            var dst = new byte[n];
            fixed (byte* pSrc = &m_buffer[m_position], pDst = dst)
            {
                byte* ps = pSrc;
                byte* pd = pDst;

                // Loop over the count in blocks of 4 bytes, copying an integer (4 bytes) at a time:
                for (int i = 0; i < n / 4; i++)
                {
                    *( (int*)pd ) = *( (int*)ps );
                    pd += 4;
                    ps += 4;
                }

                // Complete the copy by moving any bytes that weren't moved in blocks of 4:
                for (int i = 0; i < n % 4; i++)
                {
                    *pd = *ps;
                    pd++;
                    ps++;
                }
            }

            m_position += n;

            return dst;
        }

        public bool ReadBoolean()
        {
            return ReadByte() != 0;
        }

        public char ReadChar()
        {
            return (char)ReadShort();
        }

        public float ReadFloat()
        {
            int val = ReadInt();
            return *(float*)&val;
        }

        public double ReadDouble()
        {
            long val = ReadLong();
            return *(double*)&val;
        }

        public string ReadUTF()
        {
            ushort length = ReadUShort();

            byte[] bytes = ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        public string ReadUTFBytes(ushort len)
        {
            byte[] bytes = ReadBytes(len);
            return Encoding.UTF8.GetString(bytes);
        }

        public void Seek(int offset, SeekOrigin seekOrigin)
        {
            if (seekOrigin == SeekOrigin.Begin)
                m_position = offset;
            else if (seekOrigin == SeekOrigin.End)
                m_position = m_buffer.Length + offset;
            else if (seekOrigin == SeekOrigin.Current)
                m_position += offset;
        }

        public void SkipBytes(int n)
        {
            m_position += n;
        }

        public void Dispose()
        {
            
        }
    }
}