

// Generated on 04/17/2013 22:30:13
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("RankNames")]
    public class RankName : IDataObject
    {
        public const String MODULE = "RankNames";
        public int id;
        public uint nameId;
        public int order;
    }
}