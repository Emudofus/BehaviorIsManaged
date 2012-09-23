

// Generated on 09/23/2012 21:40:24
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Job")]
    public class Job : IDataObject
    {
        private const String MODULE = "Jobs";
        public int id;
        public uint nameId;
        public int specializationOfId;
        public int iconId;
        public List<int> toolIds;
    }
}