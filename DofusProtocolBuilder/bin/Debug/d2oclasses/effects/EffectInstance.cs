

// Generated on 10/25/2012 10:42:59
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("EffectInstance")]
    public class EffectInstance : IDataObject
    {
        public uint effectId;
        public int targetId;
        public int duration;
        public int delay;
        public int random;
        public int group;
        public int modificator;
        public Boolean trigger;
        public Boolean hidden;
        public uint zoneSize;
        public uint zoneShape;
        public uint zoneMinSize;
    }
}