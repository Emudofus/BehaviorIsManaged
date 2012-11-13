

// Generated on 10/25/2012 10:43:02
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("SoundBones")]
    public class SoundBones : IDataObject
    {
        public uint id;
        public List<String> keys;
        public List<List<SoundAnimation>> values;
        public String MODULE = "SoundBones";
    }
}