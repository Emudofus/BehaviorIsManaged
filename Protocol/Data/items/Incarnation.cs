

// Generated on 12/11/2012 19:44:37
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