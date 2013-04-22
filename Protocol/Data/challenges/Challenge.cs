

// Generated on 04/17/2013 22:30:12
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Challenge")]
    public class Challenge : IDataObject
    {
        public const String MODULE = "Challenge";
        public int id;
        public uint nameId;
        public uint descriptionId;
    }
}