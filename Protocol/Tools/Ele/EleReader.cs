#region License GNU GPL
// EleReader.cs
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

namespace BiM.Protocol.Tools.Ele
{
    public class EleReader: IDisposable
    {
        private BigEndianReader m_reader;
        private Stream m_stream;

        public EleReader(string filePath)
        {
            m_stream = File.OpenRead(filePath);
            m_reader = new BigEndianReader(m_stream);
        }

        public EleReader(Stream stream)
        {
            m_stream = stream;
            m_reader = new BigEndianReader(m_stream);
        }

        public EleInstance ReadElements()
        {
            m_reader.Seek(0, SeekOrigin.Begin);

            int header = m_reader.ReadByte();

            if (header != 69)
            {
                try
                {
                    var uncompress = ZipHelper.Uncompress(m_reader.ReadBytes((int)m_reader.BytesAvailable));

                    if (uncompress.Length <= 0 || uncompress[0] != 69)
                        throw new FileLoadException("Wrong header file");

                    ChangeStream(new MemoryStream(uncompress));
                }
                catch (Exception)
                {
                    throw new FileLoadException("Wrong header file");
                }
            }

            var instance = EleInstance.ReadFromStream(m_reader);

            return instance;
        }

        private void ChangeStream(Stream stream)
        {
            m_stream.Dispose();
            m_reader.Dispose();

            m_stream = stream;
            m_reader = new BigEndianReader(m_stream);
        }

        public void Dispose()
        {
            m_stream.Dispose();
            m_reader.Dispose();
        }
    }
}