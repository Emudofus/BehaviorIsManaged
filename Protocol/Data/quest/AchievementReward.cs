

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AchievementRewards")]
    public class AchievementReward : IDataObject
    {
        private const String MODULE = "AchievementRewards";
        public uint id;
        public uint achievementId;
        public int levelMin;
        public int levelMax;
        public List<List<uint>> itemsReward;
        public List<uint> emotesReward;
        public List<uint> spellsReward;
        public List<uint> titlesReward;
        public List<uint> ornamentsReward;
    }
}