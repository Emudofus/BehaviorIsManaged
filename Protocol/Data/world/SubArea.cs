

// Generated on 04/17/2013 22:30:17
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("SubAreas")]
    public class SubArea : IDataObject
    {
        public const String MODULE = "SubAreas";
        public int id;
        public uint nameId;
        public int areaId;
        public List<AmbientSound> ambientSounds;
        public List<uint> mapIds;
        public Rectangle bounds;
        public List<int> shape;
        public List<uint> customWorldMap;
        public int packId;
        public uint level;
        public Boolean displayOnWorldMap;
        public List<uint> monsters;
    }
}