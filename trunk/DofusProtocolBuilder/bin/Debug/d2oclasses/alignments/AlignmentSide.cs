

// Generated on 10/25/2012 10:42:59
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AlignmentSides")]
    public class AlignmentSide : IDataObject
    {
        private const String MODULE = "AlignmentSides";
        public int id;
        public uint nameId;
        public Boolean canConquest;
    }
}