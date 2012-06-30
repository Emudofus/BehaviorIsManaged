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