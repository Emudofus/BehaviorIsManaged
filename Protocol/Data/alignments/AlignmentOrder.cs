

// Generated on 12/11/2012 19:44:36
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AlignmentOrder")]
    public class AlignmentOrder : IDataObject
    {
        private const String MODULE = "AlignmentOrder";
        public int id;
        public uint nameId;
        public uint sideId;
    }
}