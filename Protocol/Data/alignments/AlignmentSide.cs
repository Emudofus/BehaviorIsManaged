

// Generated on 04/17/2013 22:30:11
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AlignmentSides")]
    public class AlignmentSide : IDataObject
    {
        public const String MODULE = "AlignmentSides";
        public int id;
        public uint nameId;
        public Boolean canConquest;
    }
}