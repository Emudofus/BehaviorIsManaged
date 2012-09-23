

// Generated on 09/23/2012 21:40:20
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Effect")]
    public class Effect : IDataObject
    {
        private const String MODULE = "Effects";
        public int id;
        public uint descriptionId;
        public int iconId;
        public int characteristic;
        public uint category;
        public String @operator;
        public Boolean showInTooltip;
        public Boolean useDice;
        public Boolean forceMinMax;
        public Boolean boost;
        public Boolean active;
        public Boolean showInSet;
        public int bonusType;
        public Boolean useInFight;
        public uint effectPriority;
    }
}