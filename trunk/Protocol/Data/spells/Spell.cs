

// Generated on 10/25/2012 10:43:02
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Spells")]
    public class Spell : IDataObject
    {
        private const String MODULE = "Spells";
        public int id;
        public uint nameId;
        public uint descriptionId;
        public uint typeId;
        public String scriptParams;
        public String scriptParamsCritical;
        public int scriptId;
        public int scriptIdCritical;
        public int iconId;
        public List<uint> spellLevels;
        public Boolean useParamCache = true;
    }
}