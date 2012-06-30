using System;
using System.IO;
using System.Text;

namespace BiM.Core.IO
{
    public class BigEndianReader : IDataReader, IDisposable
    {
        #region Properties

        private BinaryReader m_reader;

        /// <summary>
        ///   Gets availiable bytes number in the buffer
        /// </summary>
        public long BytesAvailable
        {
            get { return m_reader.BaseStream.Length - m_reader.BaseStream.Position; }
        }

        public Stream BaseStream
        {
            get
            {
                return m_reader.BaseStream;
            }
        }

        #endregion

        #region Initialisation

        /// <summary>
        ///   Initializes a new instance of the <see cref = "BigEndianReader" /> class.
        /// </summary>
        public BigEndianReader()
        {
            m_reader = new BinaryReader(new MemoryStream(), Encoding.UTF8);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "BigEndianReader" /> class.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        public BigEndianReader(Stream stream)
        {
            m_reader = new BinaryReader(stream, Encoding.UTF8);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "BigEndianReader" /> class.
        /// </summary>
        /// <param name = "tab">Memory buffer.</param>
        public BigEndianReader(byte[] tab)
        {
            m_reader = new BinaryReader(new MemoryStream(tab), Encoding.UTF8);
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///   Read bytes in big endian format
        /// </summary>
        /// <param name = "count"></param>
        /// <returns></returns>
        private byte[] ReadBigEndianBytes(int count)
        {
            var bytes = new byte[count];
            int i;
            for (i = count - 1; i >= 0; i--)
                bytes[i] = m_reader.ReadByte();
            return bytes;
        }

        #endregion

        #region Public Method

        /// <summary>
        ///   Read a Short from the Buffer
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            return BitConverter.ToInt16(ReadBigEndianBytes(2), 0);
        }

        /// <summary>
        ///   Read a int from the Buffer
        /// </summary>
        /// <returns></returns>
        public int ReadInt()
        {
            return BitConverter.ToInt32(ReadBigEndianBytes(4), 0);
        }

        /// <summary>
        ///   Read a long from the Buffer
        /// </summary>
        /// <returns></returns>
        public Int64 ReadLong()
        {
            return BitConverter.ToInt64(ReadBigEndianBytes(8), 0);
        }

        /// <summary>
        ///   Read a Float from the Buffer
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadBigEndianBytes(4), 0);
        }

        /// <summary>
        ///   Read a UShort from the Buffer
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(ReadBigEndianBytes(2), 0);
        }

        /// <summary>
        ///   Read a int from the Buffer
        /// </summary>
        /// <returns></returns>
        public UInt32 ReadUInt()
        {
            return BitConverter.ToUInt32(ReadBigEndianBytes(4), 0);
        }

        /// <summary>
        ///   Read a long from the Buffer
        /// </summary>
        /// <returns></returns>
        public UInt64 ReadULong()
        {
            return BitConverter.ToUInt64(ReadBigEndianBytes(8), 0);
        }

        /// <summary>
        ///   Read a byte from the Buffer
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return m_reader.ReadByte();
        }

        public sbyte ReadSByte()
        {
            return m_reader.ReadSByte();
        }

        /// <summary>
        ///   Returns N bytes from the buffer
        /// </summary>
        /// <param name = "n">Number of read bytes.</param>
        /// <returns></returns>
        public byte[] ReadBytes(int n)
        {
            return m_reader.ReadBytes(n);
        }

        /// <summary>
        /// Returns N bytes from the buffer
        /// </summary>
        /// <param name = "n">Number of read bytes.</param>
        /// <returns></returns>
        public BigEndianReader ReadBytesInNewBigEndianReader(int n)
        {
            return new BigEndianReader(m_reader.ReadBytes(n));
        }

        /// <summary>
        ///   Read a Boolean from the Buffer
        /// </summary>
        /// <returns></returns>
        public Boolean ReadBoolean()
        {
            return m_reader.ReadByte() == 1;
        }

        /// <summary>
        ///   Read a Char from the Buffer
        /// </summary>
        /// <returns></returns>
        public Char ReadChar()
        {
            return (char) ReadUShort();
        }

        /// <summary>
        ///   Read a Double from the Buffer
        /// </summary>
        /// <returns></returns>
        public Double ReadDouble()
        {
            return BitConverter.ToDouble(ReadBigEndianBytes(8), 0);
        }

        /// <summary>
        ///   Read a Single from the Buffer
        /// </summary>
        /// <returns></returns>
        public Single ReadSingle()
        {
            return BitConverter.ToSingle(ReadBigEndianBytes(4), 0);
        }

        /// <summary>
        ///   Read a string from the Buffer
        /// </summary>
        /// <returns></returns>
        public string ReadUTF()
        {
            ushort length = ReadUShort();

            byte[] bytes = ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///   Read a string from the Buffer
        /// </summary>
        /// <returns></returns>
        public string ReadUTF7BitLength()
        {
            int length = ReadInt();

            byte[] bytes = ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///   Read a string from the Buffer
        /// </summary>
        /// <returns></returns>
        public string ReadUTFBytes(ushort len)
        {
            byte[] bytes = ReadBytes(len);

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///   Skip bytes
        /// </summary>
        /// <param name = "n"></param>
        public void SkipBytes(int n)
        {
            int i;
            for (i = 0; i < n; i++)
            {
                m_reader.ReadByte();
            }
        }


        public void Seek(int offset, SeekOrigin seekOrigin)
        {
            m_reader.BaseStream.Seek(offset, seekOrigin);
        }

        /// <summary>
        ///   Add a bytes array to the end of the buffer
        /// </summary>
        public void Add(byte[] data, int offset, int count)
        {
            long pos = m_reader.BaseStream.Position;

            m_reader.BaseStream.Position = m_reader.BaseStream.Length;
            m_reader.BaseStream.Write(data, offset, count);
            m_reader.BaseStream.Position = pos;
        }

        #endregion

        #region Dispose

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            m_reader.Dispose();
            m_reader = null;
        }

        #endregion
    }
}