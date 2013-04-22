

// Generated on 04/17/2013 22:30:15
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("SpeakingItemsText")]
    public class SpeakingItemText : IDataObject
    {
        public const String MODULE = "SpeakingItemsText";
        public int textId;
        public float textProba;
        public uint textStringId;
        public int textLevel;
        public int textSound;
        public String textRestriction;
    }
}