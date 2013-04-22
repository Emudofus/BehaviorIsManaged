#region License GNU GPL
// DlmReader.cs
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
using BiM.Core.IO;
using BiM.Protocol.Data;

namespace BiM.Protocol.Tools.Dlm
{
    public class DlmReader : IDisposable
    {
        /// <summary>
        /// Returns the decryption mainLock
        /// </summary>
        /// <param name="mapId">The map to decrypt</param>
        /// <returns>The decryption mainLock</returns>
        public delegate string KeyProvider(int mapId);

        private IDataReader m_reader;

        public DlmReader(string filePath)
        {
            m_reader = new FastBigEndianReader(File.ReadAllBytes(filePath));
        }

        public DlmReader(string filePath, string decryptionKey)
        {
            m_reader = new FastBigEndianReader(File.ReadAllBytes(filePath));
            DecryptionKey = decryptionKey;
        }

        public DlmReader(IDataReader reader)
        {
            m_reader = reader;
        }

        public DlmReader(Stream stream)
        {
            m_reader = new BigEndianReader(stream);
        }

        public DlmReader(byte[] buffer)
        {
            m_reader = new FastBigEndianReader(buffer);
        }

        public string DecryptionKey
        {
            get;
            set;
        }

        public KeyProvider DecryptionKeyProvider
        {
            get;
            set;
        }

        public DlmMap ReadMap()
        {
            m_reader.Seek(0, SeekOrigin.Begin);
            int header = m_reader.ReadByte();

            if (header != 77)
            {
                try
                {
                    m_reader.Seek(0, SeekOrigin.Begin);
                    var output = new MemoryStream();
                    ZipHelper.Deflate(new MemoryStream(m_reader.ReadBytes((int) m_reader.BytesAvailable)), output);

                    var uncompress = output.ToArray();

                    m_reader.Dispose();
                    m_reader = new FastBigEndianReader(uncompress);

                    header = m_reader.ReadByte();

                    if (header != 77)
                        throw new FileLoadException("Wrong header file");

                }
                catch (Exception ex)
                {
                    throw new FileLoadException("Wrong header file");
                }
            }

            var map = DlmMap.ReadFromStream(m_reader, this);

            return map;
        }

        public void Dispose()
        {
            m_reader.Dispose();
        }
    }
}