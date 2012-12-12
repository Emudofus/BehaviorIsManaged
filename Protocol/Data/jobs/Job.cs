

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Jobs")]
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