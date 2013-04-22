

// Generated on 04/17/2013 22:30:14
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Jobs")]
    public class Job : IDataObject
    {
        public const String MODULE = "Jobs";
        public int id;
        public uint nameId;
        public int specializationOfId;
        public int iconId;
        public List<int> toolIds;
    }
}