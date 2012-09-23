

// Generated on 09/23/2012 21:40:24
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Appearance")]
    public class Appearance : IDataObject
    {
        public const String MODULE = "Appearances";
        public uint id;
        public uint type;
        public String data;
    }
}