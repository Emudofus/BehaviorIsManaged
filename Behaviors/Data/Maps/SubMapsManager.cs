﻿#region License GNU GPL
// SubMapsManager.cs
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BiM.Behaviors.Game.World;
using BiM.Behaviors.Game.World.Data;
using BiM.Behaviors.Game.World.MapTraveling;
using BiM.Behaviors.Game.World.MapTraveling.Storage;
using BiM.Behaviors.Game.World.MapTraveling.Transitions;
using BiM.Core.Reflection;
using BiM.Core.UI;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace BiM.Behaviors.Data
{
    public class SubMapsManager : Singleton<SubMapsManager>
    {
        private const string RedisSubMapsKey = "SubMaps";
        private const string RedisSubMapsByMapKey = "SubMapsByMap";

        private readonly PooledRedisClientManager m_clientManager = new PooledRedisClientManager("localhost");

        // for generation only
        private readonly ConcurrentDictionary<int, MapData> m_loadedMaps = new ConcurrentDictionary<int, MapData>();
        private readonly ConcurrentDictionary<Point, List<MapData>> m_mapsByPosition = new ConcurrentDictionary<Point, List<MapData>>();
        private readonly ConcurrentDictionary<MapData, GeneratedSubMap[]> m_submaps = new ConcurrentDictionary<MapData, GeneratedSubMap[]>();

        #region IDataSource Members

        public SerializableSubMap GetSubMap(long globalid)
        {

                using (IRedisTypedClient<SerializableSubMap> client = m_clientManager.GetClient().As<SerializableSubMap>())
                {
                    IRedisHash<long, SerializableSubMap> hash = client.GetHash<long>(RedisSubMapsKey);

                    if (!hash.ContainsKey(globalid))
                        throw new Exception(string.Format("Submap {0} not found", globalid));

                    return hash[globalid];
                }
        }

        public SerializableSubMap[] GetMapSubMaps(int mapid)
        {
            using (IRedisClient client = m_clientManager.GetClient())
            {
                IRedisTypedClient<long[]> keysClient = client.As<long[]>();
                IRedisHash<int, long[]> hash = keysClient.GetHash<int>(RedisSubMapsByMapKey);

                if (!hash.ContainsKey(mapid))
                    return new SerializableSubMap[0];

                long[] submaps = hash[mapid];
                var submapClient = (RedisTypedClient<SerializableSubMap>)client.As<SerializableSubMap>();

                return submapClient.GetValuesFromHash(submapClient.GetHash<long>(RedisSubMapsKey), submaps).ToArray();
            }
        }


        #endregion

        public ProgressionCounter Initialize()
        {
            using (IRedisClient client = m_clientManager.GetReadOnlyClient())
            {
                if (!client.ContainsKey(RedisSubMapsKey))
                {
                    // generate
                    return BeginSubMapsGeneration();
                }
            }

            return null;
        }


        public ProgressionCounter BeginSubMapsGeneration()
        {
            var progression = new ProgressionCounter(100);

            Task.Factory.StartNew(() => GenerateSubMaps(progression));

            return progression;
        }

        private void GenerateSubMaps(ProgressionCounter progression)
        {
            double total = MapsManager.Instance.MapsCount;

            progression.UpdateValue(0, "Loading all maps ...");
            int counter = 0;
            Parallel.ForEach(MapsManager.Instance.EnumerateMaps(), map =>
            {
                var builder = new SubMapBuilder<CellData, MapData>(map);
                GeneratedSubMap[] submaps = builder.BuildLight();

                m_submaps.TryAdd(map, submaps);
                m_loadedMaps.TryAdd(map.Id, map);

                var pos = new Point(map.X, map.Y);
                lock (m_mapsByPosition)
                {
                    List<MapData> list;
                    if (m_mapsByPosition.TryGetValue(pos, out list)) list.Add(map);
                    else m_mapsByPosition.TryAdd(pos, new List<MapData> { map });
                }

                // update the counter (in percent)
                Interlocked.Increment(ref counter);
                if (counter % 30 == 0)
                {
                    lock (progression)
                    {
                        if (counter % 30 == 0)
                            progression.UpdateValue(total == 0 ? 100d : ( counter / total ) * 100d);
                    }
                }
            });

            progression.UpdateValue(0, "Binding submaps together ...");
            counter = 0;
            Parallel.ForEach(m_submaps, cacheEntry =>
            {
                var neighbours = new[]
                        {
                            TryGetMapNeighbour(cacheEntry.Key, MapNeighbour.Right),
                            TryGetMapNeighbour(cacheEntry.Key, MapNeighbour.Top),
                            TryGetMapNeighbour(cacheEntry.Key, MapNeighbour.Left),
                            TryGetMapNeighbour(cacheEntry.Key, MapNeighbour.Bottom),
                        };

                foreach (GeneratedSubMap submap in cacheEntry.Value)
                {
                    for (MapNeighbour neighbour = MapNeighbour.Right; neighbour <= MapNeighbour.Bottom; neighbour++)
                    {
                        int i = (int)neighbour - 1;

                        if (neighbours[i] == null)
                            continue;

                        MapNeighbour opposite = GetOppositeDirection(neighbour);
                        GeneratedSubMap[] submaps;
                        int mapChangeData = Map.MapChangeDatas[neighbour];
                        int oppositeMapChangeData = Map.MapChangeDatas[neighbour];
                        int cellChangement = Map.MapCellChangement[neighbour];

                        if (neighbours[i] != null && m_submaps.TryGetValue(neighbours[i], out submaps))
                        {
                            lock (submaps)
                                foreach (GeneratedSubMap neighbourSubmap in submaps)
                                {
                                    // neighbor already set
                                    if (submap.SubMap.Neighbours.Any(x => x.GlobalId == neighbourSubmap.SubMap.GlobalId))
                                        continue;

                                    // if any cell of the submaps is a transition to another submap
                                    GeneratedSubMap submap1 = neighbourSubmap;
                                    var links = submap.ChangeCells.Where(x => ( x.MapChangeData & mapChangeData ) != 0 &&
                                             submap1.ChangeCells.Any(y => y.Id == x.Id + cellChangement)).Select(x => x.Id).ToArray();
                                    if (links.Length > 0)
                                    {
                                        // set in the two ways
                                        lock (submap.SubMap.Neighbours)
                                            lock (neighbourSubmap.SubMap.Neighbours)
                                            {
                                                submap.SubMap.Neighbours.Add(new SubMapNeighbour(neighbourSubmap.SubMap.GlobalId, new MovementTransition(neighbour, links)));
                                                neighbourSubmap.SubMap.Neighbours.Add(new SubMapNeighbour(submap.SubMap.GlobalId,
                                                    new MovementTransition(opposite, links.Select(x => (short)( x + cellChangement )).ToArray())));
                                            }
                                    }
                                }
                        }
                    }
                }

                // update the counter (in percent)
                Interlocked.Increment(ref counter);
                if (counter % 30 == 0)
                {
                    lock (progression)
                    {
                        if (counter % 30 == 0)
                            progression.UpdateValue(counter / (double)m_submaps.Count * 100d);
                    }
                }
            });


            using (IRedisClient client = m_clientManager.GetClient())
            {
                progression.UpdateValue(0, "Storing informations on Redis server...");

                IRedisTypedClient<SerializableSubMap> typedClient1 = client.As<SerializableSubMap>();
                typedClient1.SetRangeInHash(typedClient1.GetHash<long>(RedisSubMapsKey), m_submaps.Values.SelectMany(x => x).ToDictionary(x => x.SubMap.GlobalId, x => x.SubMap));
                progression.UpdateValue(50);
                IRedisTypedClient<long[]> typedClient2 = client.As<long[]>();
                typedClient2.SetRangeInHash(typedClient2.GetHash<int>(RedisSubMapsByMapKey), m_submaps.ToDictionary(x => x.Key.Id, x => x.Value.Select(y => y.SubMap.GlobalId).ToArray()));
                progression.UpdateValue(100);
            }

            m_submaps.Clear();
            m_loadedMaps.Clear();
            m_mapsByPosition.Clear();

            progression.NotifyEnded();
        }

        private MapNeighbour GetOppositeDirection(MapNeighbour neighbour)
        {
            switch (neighbour)
            {
                case MapNeighbour.Top:
                    return MapNeighbour.Bottom;
                case MapNeighbour.Bottom:
                    return MapNeighbour.Top;
                case MapNeighbour.Right:
                    return MapNeighbour.Left;
                case MapNeighbour.Left:
                    return MapNeighbour.Right;
                default:
                    throw new Exception(string.Format("Invalid MapNeighbour {0}", neighbour));
            }
        }

        private MapData TryGetMapNeighbour(MapData map, MapNeighbour neighbour)
        {
            int posX;
            int posY;
            int clientMapId;
            switch (neighbour)
            {
                case MapNeighbour.Top:
                    clientMapId = map.TopNeighbourId;
                    posX = map.X;
                    posY = map.Y - 1;
                    break;
                case MapNeighbour.Bottom:
                    clientMapId = map.BottomNeighbourId;
                    posX = map.X;
                    posY = map.Y + 1;
                    break;
                case MapNeighbour.Right:
                    clientMapId = map.RightNeighbourId;
                    posX = map.X + 1;
                    posY = map.Y;
                    break;
                case MapNeighbour.Left:
                    clientMapId = map.LeftNeighbourId;
                    posX = map.X - 1;
                    posY = map.Y;
                    break;
                default:
                    return null;
            }

            if (clientMapId < 0)
            {
                return null;
            }

            MapData clientMap;
            m_loadedMaps.TryGetValue(clientMapId, out clientMap);

            List<MapData> mapsFromPos;
            m_mapsByPosition.TryGetValue(new Point(posX, posY), out mapsFromPos);

            // most of the cases
            if (mapsFromPos != null && clientMap != null &&
                ( mapsFromPos.Count == 1 || mapsFromPos.Count(x => x.SubAreaId == clientMap.SubAreaId) == 1 && mapsFromPos.Any(x => x.Id == clientMap.Id) ))
                return clientMap;

            // to guess the correct map we count the number of walkable cells to avoid "display only" maps
            int clientMapCells = clientMap != null ? clientMap.Cells.Count(x => x.Walkable) : 0;

            MapData relativeMap = null;
            if (clientMap != null)
                m_loadedMaps.TryGetValue((int)clientMap.RelativeId, out relativeMap);

            int relativeMapCells = relativeMap != null ? relativeMap.Cells.Count(x => x.Walkable) : 0;

            MapData betterMap = relativeMapCells < clientMapCells ? relativeMap : clientMap;
            int betterMapCount = relativeMapCells < clientMapCells ? relativeMapCells : clientMapCells;
            if (mapsFromPos != null)
                foreach (MapData mapFromPos in mapsFromPos)
                {
                    int count = mapFromPos.Cells.Count(x => x.Walkable);
                    if (count < betterMapCount)
                    {
                        betterMap = mapFromPos;
                        betterMapCount = count;
                    }
                }

            return betterMap;
        }
    }
}