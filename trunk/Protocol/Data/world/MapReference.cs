

// Generated on 10/25/2012 10:43:02
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("MapReferences")]
    public class MapReference : IDataObject
    {
        private const String MODULE = "MapReferences";
        public int id;
        public uint mapId;
        public int cellId;
    }
}