

// Generated on 09/23/2012 21:40:19
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AlignmentEffect")]
    public class AlignmentEffect : IDataObject
    {
        private const String MODULE = "AlignmentEffect";
        public int id;
        public uint characteristicId;
        public uint descriptionId;
    }
}