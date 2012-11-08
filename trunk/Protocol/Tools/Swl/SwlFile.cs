#region License GNU GPL
// SwlFile.cs
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
using System.IO;
using BiM.Core.IO;

namespace BiM.Protocol.Tools.Swl
{
    public class SwlFile : IDisposable
    {
        private BigEndianReader m_reader;
        private Stream m_stream;

        public SwlFile(Stream stream)
        {
            m_stream = stream;
            m_reader = new BigEndianReader(m_stream);

            Classes = new List<string>();

            ReadFile();
        }

        public SwlFile(string file)
            : this(File.Open(file, FileMode.Open))
        {
        }

        public string FilePath
        {
            get;
            private set;
        }

        public byte Version
        {
            get;
            set;
        }

        public uint FrameRate
        {
            get;
            set;
        }

        public List<string> Classes
        {
            get;
            set;
        }

        public byte[] SwfData
        {
            get;
            set;
        }

        private void ReadFile()
        {
            if (m_reader.ReadByte() != 76)
            {
                throw new Exception("Malformated swf file");
            }

            Version = m_reader.ReadByte();
            FrameRate = m_reader.ReadUInt();

            var count =m_reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                Classes.Add(m_reader.ReadUTF());
            }

            SwfData = m_reader.ReadBytes((int) m_reader.BytesAvailable);
        }

        public void Dispose()
        {
            m_reader.Dispose();
        }
    }
}