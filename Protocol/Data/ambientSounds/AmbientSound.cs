

// Generated on 04/17/2013 22:30:11
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("AmbientSounds")]
    public class AmbientSound : IDataObject
    {
        public const int AMBIENT_TYPE_ROLEPLAY = 1;
        public const int AMBIENT_TYPE_AMBIENT = 2;
        public const int AMBIENT_TYPE_FIGHT = 3;
        public const int AMBIENT_TYPE_BOSS = 4;
        public const String MODULE = "AmbientSounds";
        public int id;
        public uint volume;
        public int criterionId;
        public uint silenceMin;
        public uint silenceMax;
        public int channel;
        public int type_id;
    }
}