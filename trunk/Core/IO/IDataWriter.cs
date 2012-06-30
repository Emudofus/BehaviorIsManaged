
using System;

namespace BiM.Core.IO
{
    public interface IDataWriter
    {
        byte[] Data
        {
            get;
        }

        /// <summary>
        ///   Write a Short into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteShort(short @short);

        /// <summary>
        ///   Write a int into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteInt(int @int);

        /// <summary>
        ///   Write a long into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteLong(Int64 @long);

        /// <summary>
        ///   Write a UShort into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteUShort(ushort @ushort);

        /// <summary>
        ///   Write a int into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteUInt(UInt32 @uint);

        /// <summary>
        ///   Write a long into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteULong(UInt64 @ulong);

        /// <summary>
        ///   Write a byte into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteByte(byte @byte);
        void WriteSByte(sbyte @byte);

        /// <summary>
        ///   Write a Float into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteFloat(float @float);

        /// <summary>
        ///   Write a Boolean into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteBoolean(Boolean @bool);

        /// <summary>
        ///   Write a Char into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteChar(Char @char);

        /// <summary>
        ///   Write a Double into the buffer
        /// </summary>
        void WriteDouble(Double @double);

        /// <summary>
        ///   Write a Single into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteSingle(Single @single);

        /// <summary>
        ///   Write a string into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteUTF(string str);

        /// <summary>
        ///   Write a string into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteUTFBytes(string str);

        /// <summary>
        ///   Write a bytes array into the buffer
        /// </summary>
        /// <returns></returns>
        void WriteBytes(byte[] data);

        void Clear();
    }
}