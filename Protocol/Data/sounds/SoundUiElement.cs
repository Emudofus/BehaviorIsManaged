

// Generated on 04/17/2013 22:30:16
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("SoundUiElement")]
    public class SoundUiElement : IDataObject
    {
        public uint id;
        public String name;
        public uint hookId;
        public String file;
        public uint volume;
        public String MODULE = "SoundUiElement";
    }
}