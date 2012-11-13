

// Generated on 10/25/2012 10:43:02
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Areas")]
    public class Area : IDataObject
    {
        private const String MODULE = "Areas";
        public int id;
        public uint nameId;
        public int superAreaId;
        public Boolean containHouses;
        public Boolean containPaddocks;
        public Rectangle bounds;
    }
}