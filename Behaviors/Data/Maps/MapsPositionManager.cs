#region License GNU GPL
// MapsPositionManager.cs
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
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BiM.Behaviors.Data.D2O;
using BiM.Behaviors.Game.World.Data;
using BiM.Core.UI;
using BiM.Protocol.Data;
using Point = System.Drawing.Point;

namespace BiM.Behaviors.Data.Maps
{
    public class MapsPositionManager : RedisDataManager<MapsPositionManager>
    {
        internal class RegionContainerCollection
        {
            private Dictionary<int, List<int>> m_subRegions = new Dictionary<int, List<int>>();
            private Dictionary<int, Dictionary<Point, List<MapWithPriority>>> m_mapsByPoint = new Dictionary<int, Dictionary<Point, List<MapWithPriority>>>();

            public void AddRegion(int containerId, int regionId)
            {
                if (!m_subRegions.ContainsKey(containerId))
                    m_subRegions.Add(containerId, new List<int>());

                m_subRegions[containerId].Add(regionId);
            }

            public bool ContainsRegion(int containerId, int regionId)
            {
                return m_subRegions.ContainsKey(containerId) && m_subRegions[containerId].Contains(regionId);
            }

            public void AddMap(int containerId, Point mapPos, MapWithPriority mapId)
            {
                if (!m_mapsByPoint.ContainsKey(containerId))
                    m_mapsByPoint.Add(containerId, new Dictionary<Point, List<MapWithPriority>>());

                if (!m_mapsByPoint[containerId].ContainsKey(mapPos))
                    m_mapsByPoint[containerId].Add(mapPos, new List<MapWithPriority>());

                m_mapsByPoint[containerId][mapPos].Add(mapId);
            }

            public int? GetBestMap(int containerId, int x, int y)
            {
                var pos = new Point(x, y);
                List<MapWithPriority> maps;
                if (!m_mapsByPoint[containerId].TryGetValue(pos, out maps))
                    return null;

                return maps.OrderByDescending(map => map.CellsCount).First().MapId;
            }

            public void Dispose()
            {
                m_subRegions.Clear();
                m_mapsByPoint.Clear();
            }
        }

        internal class MapWithPriority
        {
            public MapWithPriority(MapData map)
            {
                MapId = map.Id;
                CellsCount = map.Cells.Count(x => x.Walkable);
            }

            public int MapId;
            public int CellsCount;
        }

        public const int VERSION = 1;

        public const string REDIS_VERSION = "MAPS_POSITIONS_VERSION";
        public const string REDIS_KEY = "MAPS_POSITIONS";

        private bool m_initialized;

        private RegionContainerCollection m_subAreaMaps = new RegionContainerCollection();
        private RegionContainerCollection m_areaChildrens = new RegionContainerCollection();
        private RegionContainerCollection m_superAreaChildrens = new RegionContainerCollection();
        private RegionContainerCollection m_worldMapsChildrens = new RegionContainerCollection();

        private Dictionary<int, SubArea> m_subAreas = new Dictionary<int, SubArea>();
        private Dictionary<int, Area> m_areas = new Dictionary<int, Area>();
        private Dictionary<int, SuperArea> m_superAreas = new Dictionary<int, SuperArea>();

        public ProgressionCounter Initialize()
        {
            m_initialized = true;

            using (var client = GetClient())
            {
                if (!client.ContainsKey(REDIS_VERSION) || !client.ContainsKey(REDIS_KEY) || client.Get<int>(REDIS_VERSION) != VERSION)
                    return BeginGeneration();
            }

            return null;
        }

        public MapPositionData GetMapPosition(int mapId, bool @throw = true)
        {
            AssertInitialized();

            using (var client = GetClient())
            {
                var hash = client.As<MapPositionData>().GetHash<int>(REDIS_KEY);

                if (!hash.ContainsKey(mapId))
                    if (@throw)
                        throw new Exception(string.Format("Map {0} not found", mapId));
                    else
                        return null;

                return hash[mapId];
            }
        }

        public IEnumerable<MapPositionData> EnumerateAllMaps()
        {
            AssertInitialized();

            using (var client = GetClient())
            {
                var hash = client.As<MapPositionData>().GetHash<int>(REDIS_KEY);

                return client.As<MapPositionData>().GetHashValues(hash);
            }
        }

        private void AssertInitialized()
        {
            if (!m_initialized)
                throw new Exception("Cannot call this method because the class isn't initialized. Call Initialize() before");
        }

        public ProgressionCounter BeginGeneration()
        {
            var progression = new ProgressionCounter();
            var maps = MapsManager.Instance.EnumerateMaps();
            Task.Factory.StartNew(() =>
                {
                    // step 1 : load areas stuff
                    progression.UpdateValue(0, "(1/4) Getting areas ...");
                    m_subAreas = ObjectDataManager.Instance.EnumerateObjects<SubArea>().ToDictionary(x => x.id);
                    progression.UpdateValue(33);
                    m_areas = ObjectDataManager.Instance.EnumerateObjects<Area>().ToDictionary(x => x.id);
                    progression.UpdateValue(66);
                    m_superAreas = ObjectDataManager.Instance.EnumerateObjects<SuperArea>().ToDictionary(x => x.id);
                    progression.UpdateValue(100);

                    // step 2 : bind to each map his parents areas
                    progression.UpdateValue(0, "(2/4) Getting maps ...");
                    var mapsPosition = new List<MapPositionData>();
                    int counter = 0;
                    progression.Total = MapsManager.Instance.MapsCount;
                    foreach (var map in maps)
                    {
                        var pos = new Point(map.X, map.Y);
                        var subArea = m_subAreas.ContainsKey(map.SubAreaId) ? m_subAreas[map.SubAreaId] : null;
                        var area = subArea != null && m_areas.ContainsKey(subArea.areaId) ? m_areas[subArea.areaId] : null;
                        var superArea = area != null && m_subAreas.ContainsKey(area.superAreaId) ? m_superAreas[area.superAreaId] : null;

                        var mapWithPrority = new MapWithPriority(map);

                        if (subArea != null)
                        {
                            m_subAreaMaps.AddRegion(subArea.id, map.Id);
                            m_subAreaMaps.AddMap(subArea.id, pos, mapWithPrority);
                        }

                        if (area != null)
                        {
                            if (!m_areaChildrens.ContainsRegion(area.id, subArea.id))
                                m_areaChildrens.AddRegion(area.id, subArea.id);
                            m_areaChildrens.AddMap(area.id, pos, mapWithPrority);
                        }

                        if (superArea != null)
                        {
                            if (!m_superAreaChildrens.ContainsRegion(superArea.id, area.id))
                                m_superAreaChildrens.AddRegion(superArea.id, area.id);
                            m_superAreaChildrens.AddMap(superArea.id, pos, mapWithPrority);
                        }

                        int? worldmapId = superArea != null ? (int?)superArea.worldmapId : null;
                        if (superArea != null)
                        {
                            if (!m_worldMapsChildrens.ContainsRegion(worldmapId.Value, superArea.id))
                                m_worldMapsChildrens.AddRegion(worldmapId.Value, superArea.id);
                            m_worldMapsChildrens.AddMap(worldmapId.Value, pos, mapWithPrority);
                        }

                        mapsPosition.Add(new MapPositionData
                        {
                            MapId = map.Id,
                            SubAreaId = subArea != null ? subArea.id : (int?) null,
                            AreaId = area != null ? area.id : (int?) null,
                            SuperAreaId = superArea != null ? superArea.id : (int?) null,
                            WorldMapId = worldmapId,
                            X = map.X,
                            Y = map.Y
                        });
                        progression.UpdateValue(counter++);
                    }

                    progression.UpdateValue(0, "(3/4) Finding neighbours ...");
                    progression.Total = mapsPosition.Count;
                    // step 3 : found for each map his neighbours
                    foreach (var map in mapsPosition)
                    {
                        var enumerator = FindMapNeighbours(map).GetEnumerator();
                        enumerator.MoveNext();
                        map.RightNeighbourId = enumerator.Current;
                        enumerator.MoveNext();
                        map.TopNeighbourId = enumerator.Current;
                        enumerator.MoveNext();
                        map.LeftNeighbourId = enumerator.Current;
                        enumerator.MoveNext();
                        map.BottomNeighbourId = enumerator.Current;

                        Debug.Assert(!enumerator.MoveNext());
                        progression.Value++;
                    }

                    progression.UpdateValue(0, "(4/4) Saving ...");
                    // step 4 : save all the datas and dispose the allocated lists
                    using (var client = GetClient())
                    {
                        var typed = client.As<MapPositionData>();
                        typed.SetRangeInHash(typed.GetHash<int>(REDIS_KEY), mapsPosition.ToDictionary(x => x.MapId));
                        client.Set(REDIS_VERSION, VERSION);
                    }

                    // dispose
                    m_subAreaMaps.Dispose();
                    m_areaChildrens.Dispose();
                    m_superAreaChildrens.Dispose();
                    m_worldMapsChildrens.Dispose();

                    m_subAreas.Clear();
                    m_areas.Clear();
                    m_superAreas.Clear();

                    progression.NotifyEnded();
                });
            return progression;
        }

        private IEnumerable<int?> FindMapNeighbours(MapPositionData map)
        {
            // right, top, left, bottom
            yield return FindMapNeighbour(map, 1, 0);
            yield return FindMapNeighbour(map, 0, -1);
            yield return FindMapNeighbour(map, -1, 0);
            yield return FindMapNeighbour(map, 0, 1);
        }

        private int? FindMapNeighbour(MapPositionData map, int deltaX, int deltaY)
        {
            int? bySubArea = map.SubAreaId != null ? m_subAreaMaps.GetBestMap(map.SubAreaId.Value, map.X + deltaX, map.Y + deltaY) : null;
            if (bySubArea != null)
                return bySubArea;

            int? byArea = map.AreaId != null ? m_areaChildrens.GetBestMap(map.AreaId.Value, map.X + deltaX, map.Y + deltaY) : null;
            if (byArea != null)
                return byArea;

            int? bySuperArea = map.SuperAreaId != null ? m_superAreaChildrens.GetBestMap(map.SuperAreaId.Value, map.X + deltaX, map.Y + deltaY) : null;
            if (bySuperArea != null)
                return bySuperArea;

            int? byWorldMap = map.WorldMapId != null ? m_worldMapsChildrens.GetBestMap(map.WorldMapId.Value, map.X + deltaX, map.Y + deltaY) : null;
            if (byWorldMap != null)
                return byWorldMap;

            return null;
        }
    }
}