using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Here are grouped all stuff that still need to be done for proper integration in BIM
/// </summary>
namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{

    /// <summary>
    /// MapDataManager provides easy access to Map data. 
    /// It provides fast reading of headers, and only load complete map data as needed. 
    /// </summary>
    public class MapDataManager
    {
        public MapDataManager(string pathToMapsData, bool StoredInMap)
        {
            throw new NotImplementedException();
        }

        public Dictionary<object, Map> Maps
        {
            get;
            private set;
        }

        public int NbMapsInNativeFiles { get; set; }


    }

    public class MapParser
    {
    }

    public class PakFile
    {
        internal bool ExistsFile(int mapId)
        {
            throw new NotImplementedException();
        }
    }
}
