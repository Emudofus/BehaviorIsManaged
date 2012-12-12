

// Generated on 12/11/2012 19:44:39
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