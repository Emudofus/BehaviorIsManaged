

// Generated on 09/23/2012 21:40:24
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Mount")]
    public class Mount : IDataObject
    {
        public uint id;
        public uint nameId;
        public String look;
        private String MODULE = "Mounts";
    }
}