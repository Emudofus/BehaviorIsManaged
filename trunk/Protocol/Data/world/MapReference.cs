

// Generated on 09/23/2012 21:40:25
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("MapReference")]
    public class MapReference : IDataObject
    {
        private const String MODULE = "MapReferences";
        public int id;
        public uint mapId;
        public int cellId;
    }
}