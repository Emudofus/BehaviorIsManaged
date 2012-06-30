using System;

namespace BiM.Protocol.Data
{
    [Serializable]
    public class Rectangle : IDataObject
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public int top;
        public int right;
        public int left;
        public int bottom;
        public Point bottomRight;
        public Point size;
        public Point topLeft;
    }
}