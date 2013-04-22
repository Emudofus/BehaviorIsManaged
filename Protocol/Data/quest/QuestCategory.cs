

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("QuestCategory")]
    public class QuestCategory : IDataObject
    {
        public const String MODULE = "QuestCategory";
        public uint id;
        public uint nameId;
        public uint order;
        public List<uint> questIds;
    }
}