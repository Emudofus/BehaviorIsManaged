

// Generated on 10/25/2012 10:43:00
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Incarnation")]
    public class Incarnation : IDataObject
    {
        private const String MODULE = "Incarnation";
        public uint id;
        public String lookMale;
        public String lookFemale;
    }
}