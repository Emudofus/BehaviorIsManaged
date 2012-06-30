
using System;

namespace BiM.Protocol
{
    public static class BooleanByteWrapper
    {
        public static byte SetFlag(byte flag, byte offset, bool value)
        {
            if (offset >= 8)
                throw new ArgumentException("offset must be lesser than 8");

            return value ? (byte) (flag | (1 << offset)) : (byte)(flag & 255 - (1 << offset));
        }

        public static byte SetFlag(int flag, byte offset, bool value)
        {
            if (offset >= 8)
                throw new ArgumentException("offset must be lesser than 8");

            return value ? (byte)(flag | (1 << offset)) : (byte)(flag & 255 - (1 << offset));
        }

        public static bool GetFlag(byte flag, byte offset)
        {
            if (offset >= 8)
                throw new ArgumentException("offset must be lesser than 8");

            return (flag & (byte) (1 << offset)) != 0;
        }
    }
}