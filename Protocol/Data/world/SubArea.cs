

// Generated on 12/11/2012 19:44:39
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("SubAreas")]
    public class SubArea : IDataObject
    {
        private const String MODULE = "SubAreas";
        public int id;
        public uint nameId;
        public int areaId;
        public List<AmbientSound> ambientSounds;
        public List<uint> mapIds;
        public Rectangle bounds;
        public List<int> shape;
        public List<uint> customWorldMap;
        public int packId;
    }
}