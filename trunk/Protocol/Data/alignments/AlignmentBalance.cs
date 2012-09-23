

// Generated on 09/23/2012 21:40:19
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AlignmentBalance")]
    public class AlignmentBalance : IDataObject
    {
        private const String MODULE = "AlignmentBalance";
        public int id;
        public int startValue;
        public int endValue;
        public uint nameId;
        public uint descriptionId;
    }
}