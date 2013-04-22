

// Generated on 04/17/2013 22:30:12
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Effects")]
    public class Effect : IDataObject
    {
        public const String MODULE = "Effects";
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