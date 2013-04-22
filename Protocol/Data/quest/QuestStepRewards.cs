

// Generated on 04/17/2013 22:30:16
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("QuestStepRewards")]
    public class QuestStepRewards : IDataObject
    {
        public const String MODULE = "QuestStepRewards";
        public uint id;
        public uint stepId;
        public int levelMin;
        public int levelMax;
        public List<List<uint>> itemsReward;
        public List<uint> emotesReward;
        public List<uint> jobsReward;
        public List<uint> spellsReward;
    }
}