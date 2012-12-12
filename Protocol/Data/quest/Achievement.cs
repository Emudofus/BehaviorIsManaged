

// Generated on 12/11/2012 19:44:38
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Achievements")]
    public class Achievement : IDataObject
    {
        private const String MODULE = "Achievements";
        public uint id;
        public uint nameId;
        public uint categoryId;
        public uint descriptionId;
        public int iconId;
        public uint points;
        public uint level;
        public uint order;
        public float kamasRatio;
        public float experienceRatio;
        public Boolean kamasScaleWithPlayerLevel;
        public List<int> objectiveIds;
        public List<int> rewardIds;
    }
}