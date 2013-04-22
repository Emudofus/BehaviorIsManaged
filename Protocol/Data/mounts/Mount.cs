

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Mounts")]
    public class Mount : IDataObject
    {
        public uint id;
        public uint nameId;
        public String look;
        private String MODULE = "Mounts";
    }
}