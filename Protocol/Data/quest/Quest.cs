

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Quests")]
    public class Quest : IDataObject
    {
        public const String MODULE = "Quests";
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