using System;
using System.IO;

namespace BiM.Core.IO
{
    public interface IDataReader
    {
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
        ///   Read a Float from the Buffer
        /// </summary>
        /// <returns></returns>
        float ReadFloat();

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
        ///   Read a Single from the Buffer
        /// </summary>
        /// <returns></returns>
        Single ReadSingle();

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