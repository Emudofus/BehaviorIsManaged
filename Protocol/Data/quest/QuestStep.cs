

// Generated on 04/17/2013 22:30:16
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("QuestSteps")]
    public class QuestStep : IDataObject
    {
        public const String MODULE = "QuestSteps";
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