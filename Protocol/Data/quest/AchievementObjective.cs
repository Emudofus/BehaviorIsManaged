

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AchievementObjectives")]
    public class AchievementObjective : IDataObject
    {
        private const String MODULE = "AchievementObjectives";
        public uint id;
        public uint achievementId;
        public uint nameId;
        public String criterion;
    }
}