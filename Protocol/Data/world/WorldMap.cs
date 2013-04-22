

// Generated on 04/17/2013 22:30:17
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("WorldMaps")]
    public class WorldMap : IDataObject
    {
        public const String MODULE = "WorldMaps";
        public int id;
        public int origineX;
        public int origineY;
        public float mapWidth;
        public float mapHeight;
        public uint horizontalChunck;
        public uint verticalChunck;
        public Boolean viewableEverywhere;
        public float minScale;
        public float maxScale;
        public float startScale;
        public int centerX;
        public int centerY;
        public int totalWidth;
        public int totalHeight;
        public List<String> zoom;
    }
}