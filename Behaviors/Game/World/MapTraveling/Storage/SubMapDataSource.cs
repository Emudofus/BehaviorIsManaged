#region License GNU GPL

// SubMapRetriever.cs
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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BiM.Behaviors.Data;
using BiM.Behaviors.Game.World.Data;
using BiM.Behaviors.Game.World.MapTraveling.Transitions;
using BiM.Core.UI;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace BiM.Behaviors.Game.World.MapTraveling.Storage
{
    /// <summary>
    /// Thread safe
    /// </summary>
    public class SubMapDataSource : IDataSource
    {
        private const string RedisSubMapsKey = "SubMaps";
        private const string RedisSubMapsByMapKey = "SubMapsByMap";
        private readonly PooledRedisClientManager m_clientManager = new PooledRedisClientManager("localhost")
            {
                NamespacePrefix = "BiM::Data.",
            };

        // for generation only
        private readonly ConcurrentDictionary<MapData, GeneratedSubMap[]> m_submaps = new ConcurrentDictionary<MapData, GeneratedSubMap[]>();
        private readonly ConcurrentDictionary<int, MapData> m_loadedMaps = new ConcurrentDictionary<int, MapData>();
        private readonly ConcurrentDictionary<Point, List<MapData>> m_mapsByPosition = new ConcurrentDictionary<Point, List<MapData>>();

        public SubMapDataSource()
        {
        }


        #region IDataSource Members

        public T ReadObject<T>(params object[] keys) where T : class
        {
            if (keys.Length != 1 || !(keys[0] is IConvertible))
                throw new ArgumentException("SubMapDataSource needs a int/long key, use ReadObject(int/long)");

            if (!DoesHandleType(typeof (T)))
                throw new ArgumentException("typeof(T)");

            if (typeof(T) == typeof(SerializableSubMap[]))
            {
                int mapid = Convert.ToInt32(keys[0]);

                using (var client = m_clientManager.GetClient())
                {
                    var keysClient = client.As<long[]>();
                    var hash = keysClient.GetHash<int>(RedisSubMapsByMapKey);

                    if (!hash.ContainsKey(mapid))
                        return new SerializableSubMap[0] as T;

                    var submaps = hash[mapid];
                    var submapClient = (RedisTypedClient<SerializableSubMap>)client.As<SerializableSubMap>();

                    return submapClient.GetValuesFromHash(submapClient.GetHash<long>(RedisSubMapsKey), submaps).ToArray() as T;
                }
            }
            else if (typeof(T) == typeof(SerializableSubMap))
            {
                long globalid = Convert.ToInt64(keys[0]);

                using (var client = m_clientManager.GetClient().As<SerializableSubMap>())
                {
                    var hash = client.GetHash<long>(RedisSubMapsKey);

                    if (!hash.ContainsKey(globalid))
                        throw new Exception(string.Format("Submap {0} not found", globalid));

                    return hash[globalid] as T;
                }
            }

            throw new ArgumentException("typeof(T)");
        }

        public IEnumerable<T> EnumerateObjects<T>(params object[] keys) where T : class
        {
            throw new NotImplementedException();
            /*
            if (!DoesHandleType(typeof(T)))
                throw new ArgumentException("typeof(T)");

            if (typeof(T) == typeof(SerializableSubMap[]))
            {
                using (var client = m_clientManager.GetClient().As<SerializableSubMap[]>())
                {
                    var hash = client.GetHash<int>(RedisSubMapsKey);

                    foreach (var entry in hash.GetAll().Values)
                    {
                        yield return entry as T;
                    }
                }
            }
            else if (typeof(T) == typeof(SerializableSubMap))
            {
                using (var client = m_clientManager.GetClient().As<SerializableSubMap>())
                {
                    var hash = client.GetHash<long>(RedisSubMapsKey);

                    foreach (var entry in hash.GetAll().Values)
                    {
                        yield return entry as T;
                    }
                }
            }*/
        }

        public bool DoesHandleType(Type type)
        {
            return type == typeof (SerializableSubMap[]) || type == typeof(SerializableSubMap);
        }

        #endregion

        public ProgressionCounter Initialize()
        {
            using (var client = m_clientManager.GetReadOnlyClient())
            {
                if (!client.ContainsKey("SubMaps"))
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
            double total = CountMaps();

            progression.UpdateValue(0, "Loading all maps ...");
            int counter = 0;
            Parallel.ForEach(DataProvider.Instance.EnumerateAll<MapData>(), map =>
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
                if (counter % 100 == 0)
                {
                    lock (progression)
                    {
                        if (counter % 100 == 0)
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

                foreach (var submap in cacheEntry.Value)
                {
                    for (int i = 1; i < 5; i++)
                    {
                        var neighbour = (MapNeighbour)i;
                        var opposite = GetOppositeDirection(neighbour);
                        GeneratedSubMap[] submaps;
                        var mapChangeData = Map.MapChangeDatas[neighbour];
                        var oppositeMapChangeData = Map.MapChangeDatas[neighbour];
                        var cellChangement =  Map.MapCellChangement[neighbour];
                        var predicate = new Func<ICell, GeneratedSubMap, bool>((cell, neighbourSupmap) =>
                        {
                            //ICell dest;
                            return (cell.MapChangeData & mapChangeData) != 0 &&
                                   neighbourSupmap.ChangeCells.Any(x => x.Id == cell.Id + cellChangement);
                        });

                        if (neighbours[i - 1] != null && m_submaps.TryGetValue(neighbours[i - 1], out submaps))
                        {
                            foreach (var neighbourSubmap in submaps)
                            {
                                // neighbor already set
                                if (submap.SubMap.Neighbours.Any(x => x.GlobalId == neighbourSubmap.SubMap.GlobalId))
                                    continue;

                                // if any cell of the submaps is a transition to another submap
                                if (submap.ChangeCells.Any(x => predicate(x, neighbourSubmap)))
                                {
                                    // set in the two ways
                                    submap.SubMap.Neighbours.Add(new SubMapNeighbour(neighbourSubmap.SubMap.GlobalId, new MovementTransition(neighbour)));
                                    neighbourSubmap.SubMap.Neighbours.Add(new SubMapNeighbour(submap.SubMap.GlobalId, new MovementTransition(opposite)));
                                }
                            }
                        }
                    }
                }

                counter++;
                // update the counter (in percent)
                Interlocked.Increment(ref counter);
                if (counter % 100 == 0)
                {
                    lock (progression)
                    {
                        if (counter % 100 == 0)
                            progression.UpdateValue(counter / (double)m_submaps.Count * 100d);
                    }
                }
            });


            using (var client = m_clientManager.GetClient())
            {
                progression.UpdateValue(0, "Storing informations on Redis server...");

                var typedClient1 = client.As<SerializableSubMap>();
                typedClient1.SetRangeInHash(typedClient1.GetHash<long>("SubMaps"), m_submaps.Values.SelectMany(x => x).ToDictionary(x => x.SubMap.GlobalId, x => x.SubMap));
                progression.UpdateValue(50);
                var typedClient2 = client.As<long[]>();
                typedClient2.SetRangeInHash(typedClient2.GetHash<int>("SubMapsByMap"), m_submaps.ToDictionary(x => x.Key.Id, x => x.Value.Select(y => y.SubMap.GlobalId).ToArray()));
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
                (mapsFromPos.Count == 1 || mapsFromPos.Count(x => x.SubAreaId == clientMap.SubAreaId) == 1 && mapsFromPos[0].Id == clientMap.Id))
                return clientMap;

            // to guess the correct map we count the number of walkable cells to avoid "display only" maps
            int clientMapCells = clientMap != null ? clientMap.Cells.Count(x => x.Walkable) : 0;

            MapData relativeMap = null;
            if (clientMap != null)
                m_loadedMaps.TryGetValue((int) clientMap.RelativeId, out relativeMap);

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

        private int CountMaps()
        {
            D2PSource source = DataProvider.Instance.Sources.OfType<D2PSource>().
                FirstOrDefault(x => x.Reader != null && x.Reader.FileName.Contains("maps"));

            if (source != null)
                return source.Reader.Entries.Count();

            return 0;
        }
    }
}