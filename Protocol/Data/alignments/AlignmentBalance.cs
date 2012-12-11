

// Generated on 12/11/2012 19:44:36
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