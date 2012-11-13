

// Generated on 10/25/2012 10:43:01
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("QuestCategory")]
    public class QuestCategory : IDataObject
    {
        private const String MODULE = "QuestCategory";
        public uint id;
        public uint nameId;
        public uint order;
        public List<uint> questIds;
    }
}