using System;
using System.IO;
using BiM.Core.IO;
using BiM.Protocol.Data;

namespace BiM.Protocol.Tools.Dlm
{
    public class DlmReader : IDisposable
    {
        private BigEndianReader m_reader;
        private Stream m_stream;

        public DlmReader(string filePath)
        {
            m_stream = File.OpenRead(filePath);
            m_reader = new BigEndianReader(m_stream);
        }

        public DlmReader(Stream stream)
        {
            m_stream = stream;
            m_reader = new BigEndianReader(m_stream);
        }

        public DlmReader(string filePath, string decryptionKey)
        {
            m_stream = File.OpenRead(filePath);
            m_reader = new BigEndianReader(m_stream);
            DecryptionKey = decryptionKey;
        }

        public DlmReader(Stream stream, string decryptionKey)
        {
            m_stream = stream;
            m_reader = new BigEndianReader(m_stream);
            DecryptionKey = decryptionKey;
        }

        public string DecryptionKey
        {
            get;
            set;
        }

        public Func<int, string> DecryptionKeyProvider
        {
            get;
            set;
        }

        public DlmMap ReadMap()
        {
            int header = m_reader.ReadByte();

            if (header != 77)
            {
                try
                {
                    m_reader.Seek(0, SeekOrigin.Begin);
                    var output = new MemoryStream();
                    ZipHelper.Deflate(new MemoryStream(m_reader.ReadBytes((int) m_reader.BytesAvailable)), output);

                    var uncompress = output.ToArray();

                    ChangeStream(new MemoryStream(uncompress));

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

        internal void ChangeStream(Stream stream)
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