

// Generated on 10/25/2012 10:43:01
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Quests")]
    public class Quest : IDataObject
    {
        private const String MODULE = "Quests";
        public uint id;
        public uint nameId;
        public List<uint> stepIds;
        public uint categoryId;
        public Boolean isRepeatable;
        public uint repeatType;
        public uint repeatLimit;
        public Boolean isDungeonQuest;
        public uint levelMin;
        public uint levelMax;
    }
}