

// Generated on 04/17/2013 22:30:16
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("Dungeons")]
    public class Dungeon : IDataObject
    {
        public const String MODULE = "Dungeons";
        public int id;
        public uint nameId;
        public int optimalPlayerLevel;
        public List<int> mapIds;
        public int entranceMapId;
        public int exitMapId;
    }
}