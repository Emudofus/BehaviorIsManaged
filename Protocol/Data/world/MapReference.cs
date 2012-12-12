

// Generated on 12/11/2012 19:44:39
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