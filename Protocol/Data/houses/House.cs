

// Generated on 12/11/2012 19:44:36
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Houses")]
    public class House : IDataObject
    {
        private const String MODULE = "Houses";
        public int typeId;
        public uint defaultPrice;
        public int nameId;
        public int descriptionId;
        public int gfxId;
    }
}