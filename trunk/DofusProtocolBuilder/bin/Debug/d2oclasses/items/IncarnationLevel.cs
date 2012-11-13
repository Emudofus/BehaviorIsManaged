

// Generated on 10/25/2012 10:43:00
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("IncarnationLevels")]
    public class IncarnationLevel : IDataObject
    {
        private const String MODULE = "IncarnationLevels";
        public int id;
        public int incarnationId;
        public int level;
        public uint requiredXp;
    }
}