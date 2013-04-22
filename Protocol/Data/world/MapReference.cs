

// Generated on 04/17/2013 22:30:16
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("MapReferences")]
    public class MapReference : IDataObject
    {
        public const String MODULE = "MapReferences";
        public int id;
        public uint mapId;
        public int cellId;
    }
}