

// Generated on 09/23/2012 21:40:19
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Challenge")]
    public class Challenge : IDataObject
    {
        private const String MODULE = "Challenge";
        public int id;
        public uint nameId;
        public uint descriptionId;
    }
}