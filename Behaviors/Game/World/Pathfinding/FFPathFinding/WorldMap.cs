#region License GNU GPL
// Spell.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

// Author : FastFrench - antispam@laposte.net
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using BiM.Behaviors.Game.World.Data;

namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
  public class WorldMap
  {
    public enum Direction { Left, Right, Top, Bottom };
    static public IEnumerable<Direction> EnumDirections { get { return (IEnumerable<Direction>)System.Enum.GetValues(typeof(Direction)); } }
    
    static private WorldMap _instance;
    static public WorldMap Instance { get { if (_instance == null) _instance = new WorldMap(_mapDataManager); return _instance; } }
    public TimeSpan ProcessAllMapsTimer { get; private set; }

    private Dictionary<int, int> MapIdToInternal;
    private int[] InternalToMapId;

    public Dictionary<int, List<int>> WorldMapConnectionsStored { get; private set; }
    public Dictionary<int, List<int>> WorldMapConnectionsComputed { get; private set; }

    static private IWorldMapProvider _mapDataManager;

    //private SortedSet<int> _mapsInTheWorld;

    private WorldMap(IWorldMapProvider mapDataManager)
    {
        if (mapDataManager == null)
            throw new ArgumentNullException("mapDataManager", "You need to provide the instance of an object to retrieve map data");
        IWorldMapProvider _mapDataManager = mapDataManager;
        WorldMapConnectionsStored = GetWorldMapConnections(_mapDataManager, true);
        WorldMapConnectionsComputed = GetWorldMapConnections(_mapDataManager, false);
    }

    private List<int> GetConnectedMaps(IMap map, bool GetStoredData)
    {
        List<int> list = new List<int>();
        foreach (var dir in EnumDirections)
        {
            int mapId = map.GetNeighbourMapId(dir, GetStoredData);
            if (mapId != -1)
                if (_mapDataManager.KnownMapIds.Contains(mapId))
                    list.Add(mapId);
        }
        return list; 
    }

    public Dictionary<int, List<int>> GetWorldMapConnections(bool StoredInMap)
    {
        if (StoredInMap)
            return WorldMapConnectionsStored;
        return WorldMapConnectionsComputed;
    }
    private Dictionary<int, List<int>> GetWorldMapConnections(IWorldMapProvider mapDataManager, bool StoredInMap)
    {
      _mapDataManager = mapDataManager;
      Stopwatch st = new Stopwatch();
      st.Start();
      //MapDataManager mapDataManager = new MapDataManager(pathToMapsData, StoredInMap); // Gets only Headers, except if checking cell content is needed
      Dictionary<int, List<int>> dico = new Dictionary<int, List<int>>(mapDataManager.KnownMapIds.Count);
      foreach (int mapId in mapDataManager.KnownMapIds)
      {
        IMap mapData = mapDataManager.LoadMap(mapId);
        dico.Add(mapData.Id, GetConnectedMaps(mapData, StoredInMap));
      }
      st.Stop();
      ProcessAllMapsTimer = st.Elapsed;

      // To do : detect submaps
      // To do : find connections between submaps 

      // Convert into 0 based internal index (todo : split sub maps)
      MapIdToInternal = new Dictionary<int, int>();
      int i=0;
      List<int> tmpInternaltoMapId = new List<int>();
      foreach (int mapId in dico.Keys) // 
      {
          MapIdToInternal[mapId] = i++;
          tmpInternaltoMapId.Add(mapId);
      }
      InternalToMapId = tmpInternaltoMapId.ToArray();
      // Fill converted int[][] to feed the PathFinder
      List<int[]> mainList = new List<int[]>();
      List<int> connectionList; 
      List<int> ConvertedConnectionList; 
      foreach (var map in dico)
      {
          int mapID = map.Key;
          connectionList = map.Value;
          ConvertedConnectionList = new List<int>();
          foreach (int mapId in connectionList)
              ConvertedConnectionList.Add(MapIdToInternal[mapId]);
      }
      return dico;
    }
  }
}
