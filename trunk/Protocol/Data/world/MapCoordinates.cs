

// Generated on 09/23/2012 21:40:25
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