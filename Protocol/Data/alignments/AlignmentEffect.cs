

// Generated on 04/17/2013 22:30:11
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AlignmentEffect")]
    public class AlignmentEffect : IDataObject
    {
        public const String MODULE = "AlignmentEffect";
        public int id;
        public uint characteristicId;
        public uint descriptionId;
    }
}