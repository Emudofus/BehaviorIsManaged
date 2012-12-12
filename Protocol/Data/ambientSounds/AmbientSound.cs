

// Generated on 12/11/2012 19:44:36
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
        private const String MODULE = "AmbientSounds";
        public int id;
        public uint volume;
        public int criterionId;
        public uint silenceMin;
        public uint silenceMax;
        public int channel;
        public int type_id;
    }
}