

// Generated on 12/11/2012 19:44:36
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Ornaments")]
    public class Ornament : IDataObject
    {
        private const String MODULE = "Ornaments";
        public int id;
        public uint nameId;
        public Boolean visible;
        public int assetId;
        public int iconId;
        public int rarity;
        public int order;
    }
}