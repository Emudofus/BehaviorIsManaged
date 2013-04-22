

// Generated on 04/17/2013 22:30:13
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Incarnation")]
    public class Incarnation : IDataObject
    {
        public const String MODULE = "Incarnation";
        public uint id;
        public String lookMale;
        public String lookFemale;
    }
}