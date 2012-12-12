

// Generated on 12/11/2012 19:44:39
using System;
using System.Collections.Generic;
using BiM.Protocol.Tools;

namespace BiM.Protocol.Data
{
    [D2OClass("MapCoordinates")]
    public class MapCoordinates : IDataObject
    {
        private const String MODULE = "MapCoordinates";
        public uint compressedCoords;
        public List<int> mapIds;
    }
}