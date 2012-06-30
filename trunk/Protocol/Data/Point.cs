using System;

namespace BiM.Protocol.Data
{
    [Serializable]
    public class Point : IDataObject
    {
        public int x;
        public int y;
        public double length;
    }
}