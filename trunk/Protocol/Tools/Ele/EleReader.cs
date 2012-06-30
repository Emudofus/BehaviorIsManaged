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