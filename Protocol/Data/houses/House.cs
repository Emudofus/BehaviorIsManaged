

// Generated on 04/17/2013 22:30:13
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Houses")]
    public class House : IDataObject
    {
        public const String MODULE = "Houses";
        public int typeId;
        public uint defaultPrice;
        public int nameId;
        public int descriptionId;
        public int gfxId;
    }
}