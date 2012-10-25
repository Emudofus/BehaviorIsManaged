

// Generated on 10/25/2012 10:43:01
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Appearances")]
    public class Appearance : IDataObject
    {
        public const String MODULE = "Appearances";
        public uint id;
        public uint type;
        public String data;
    }
}