

// Generated on 10/25/2012 10:43:00
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("ItemSets")]
    public class ItemSet : IDataObject
    {
        private const String MODULE = "ItemSets";
        public uint id;
        public List<uint> items;
        public uint nameId;
        public List<List<EffectInstance>> effects;
        public Boolean bonusIsSecret;
    }
}