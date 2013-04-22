

// Generated on 04/17/2013 22:30:16
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Hints")]
    public class Hint : IDataObject
    {
        public const String MODULE = "Hints";
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