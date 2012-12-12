

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("QuestSteps")]
    public class QuestStep : IDataObject
    {
        private const String MODULE = "QuestSteps";
        public uint id;
        public uint questId;
        public uint nameId;
        public uint descriptionId;
        public int dialogId;
        public uint optimalLevel;
        public float duration;
        public Boolean kamasScaleWithPlayerLevel;
        public float kamasRatio;
        public float xpRatio;
        public List<uint> objectiveIds;
        public List<uint> rewardsIds;
    }
}