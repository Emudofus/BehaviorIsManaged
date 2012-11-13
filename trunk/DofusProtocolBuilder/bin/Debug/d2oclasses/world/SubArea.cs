

// Generated on 10/25/2012 10:43:02
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