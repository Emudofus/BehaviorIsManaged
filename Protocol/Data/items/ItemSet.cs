

// Generated on 04/17/2013 22:30:13
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("ItemSets")]
    public class ItemSet : IDataObject
    {
        public const String MODULE = "ItemSets";
        public uint id;
        public List<uint> items;
        public uint nameId;
        public List<List<EffectInstance>> effects;
        public Boolean bonusIsSecret;
    }
}