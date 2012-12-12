

// Generated on 12/11/2012 19:44:39
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Hints")]
    public class Hint : IDataObject
    {
        private const String MODULE = "Hints";
        public int id;
        public uint categoryId;
        public uint gfx;
        public uint nameId;
        public uint mapId;
        public uint realMapId;
        public int x;
        public int y;
        public Boolean outdoor;
        public int subareaId;
    }
}