#region License GNU GPL

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
using BiM.Behaviors.Game.World.MapTraveling.Transitions;
using BiM.Core.Reflection;
using BiM.Core.UI;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace BiM.Behaviors.Data.Maps
{
    public class SubMapsManager : Singleton<SubMapsManager>
    {
        public const int VERSION = 1;

        public const string REDIS_VERSION = "SUBMAPS_VERSION";
        public const string REDIS_KEY = "SUBMAPS";
        public const string REDIS_MAPS = "SUBMAPS_MAPS";

        private bool m_initialized;

        private readonly PooledRedisClientManager m_clientManager = new PooledRedisClientManager("localhost");

        // for generation only
        private Dictionary<int, MapPositionData> m_mapsPosition = new Dictionary<int, MapPositionData>();
        private readonly ConcurrentDictionary<int, AdjacentSubMap[]> m_submaps = new ConcurrentDictionary<int, AdjacentSubMap[]>();

        public SubMapBinder GetSubMapBinder(long globalid)
        {
            AssertInitialized();

            using (var client = m_clientManager.GetClient())
            {
                IRedisHash<long, SubMapBinder> hash = client.As<SubMapBinder>().GetHash<long>(REDIS_KEY);

                if (!hash.ContainsKey(globalid))
                    throw new Exception(string.Format("Submap {0} not found", globalid));

                return hash[globalid];
            }
        }

        public SubMapBinder[] GetMapSubMapsBinder(int mapid)
        {
            AssertInitialized();

            using (IRedisClient client = m_clientManager.GetClient())
            {
                IRedisTypedClient<long[]> keysClient = client.As<long[]>();
                IRedisHash<int, long[]> hash = keysClient.GetHash<int>(REDIS_MAPS);

                if (!hash.ContainsKey(mapid))
                    return new SubMapBinder[0];

                long[] submaps = hash[mapid];
                var submapClient = (RedisTypedClient<SubMapBinder>)client.As<SubMapBinder>();

                return submapClient.GetValuesFromHash(submapClient.GetHash<long>(REDIS_KEY), submaps).ToArray();
            }
        }

        private void AssertInitialized()
        {
            if (!m_initialized)
                throw new Exception("Cannot call this method because the class isn't initialized. Call Initialize() before");
        }

        public ProgressionCounter Initialize()
        {
            m_initialized = true;

            using (IRedisClient client = m_clientManager.GetReadOnlyClient())
            {
                if (!client.ContainsKey(REDIS_VERSION) || !client.ContainsKey(REDIS_MAPS) || !client.ContainsKey(REDIS_KEY))
                    return BeginSubMapsGeneration();

                var remoteVersion = client.Get<int>(REDIS_VERSION);

                if (remoteVersion != VERSION)
                    return BeginSubMapsGeneration();
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
            m_mapsPosition = MapsPositionManager.Instance.EnumerateAllMaps().ToDictionary(x => x.MapId);
            int counter = 0;
            Parallel.ForEach(MapsManager.Instance.EnumerateMaps(), map =>
                {
                    var builder = new SubMapBuilder();
                    AdjacentSubMap[] submaps = builder.GenerateBinders(map);

                    m_submaps.TryAdd(map.Id, submaps);

                    // update the counter (in percent)
                    Interlocked.Increment(ref counter);
                    if (counter%30 == 0)
                    {
                        lock (progression)
                        {
                            if (counter%30 == 0)
                                progression.UpdateValue(total == 0 ? 100d : (counter/total)*100d);
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

                    foreach (AdjacentSubMap submap in cacheEntry.Value)
                    {
                        for (MapNeighbour neighbour = MapNeighbour.Right; neighbour <= MapNeighbour.Bottom; neighbour++)
                        {
                            int i = (int) neighbour - 1;

                            if (neighbours[i] == null)
                                continue;

                            MapNeighbour opposite = GetOppositeDirection(neighbour);
                            AdjacentSubMap[] submaps;
                            int mapChangeData = Map.MapChangeDatas[neighbour];
                            int oppositeMapChangeData = Map.MapChangeDatas[neighbour];
                            int cellChangement = Map.MapCellChangement[neighbour];

                            if (neighbours[i] != null && m_submaps.TryGetValue(neighbours[i].Value, out submaps))
                            {
                                lock (submaps)
                                    foreach (AdjacentSubMap neighbourSubmap in submaps)
                                    {
                                        // neighbor already set
                                        lock (submap.SubMap.Neighbours)
                                            if (submap.SubMap.Neighbours.Any(x => x.GlobalId == neighbourSubmap.SubMap.GlobalId))
                                                continue;

                                        // if any cell of the submaps is a transition to another submap
                                        AdjacentSubMap submap1 = neighbourSubmap;
                                        short[] links = submap.ChangeCells.Where(x => (x.MapChangeData & mapChangeData) != 0 &&
                                                                                      submap1.ChangeCells.Any(y => y.Id == x.Id + cellChangement)).Select(x => x.Id).ToArray();
                                        if (links.Length > 0)
                                        {
                                            // set in the two ways
                                            lock (submap.SubMap.Neighbours)
                                                lock (neighbourSubmap.SubMap.Neighbours)
                                                {
                                                    submap.SubMap.Neighbours.Add(new SubMapNeighbour(neighbourSubmap.SubMap.GlobalId, new MovementTransition(neighbour, links)));
                                                    neighbourSubmap.SubMap.Neighbours.Add(new SubMapNeighbour(submap.SubMap.GlobalId,
                                                                                                              new MovementTransition(opposite, links.Select(x => (short) (x + cellChangement)).ToArray())));
                                                }
                                        }
                                    }
                            }
                        }
                    }

                    // update the counter (in percent)
                    Interlocked.Increment(ref counter);
                    if (counter%30 == 0)
                    {
                        lock (progression)
                        {
                            if (counter%30 == 0)
                                progression.UpdateValue(counter/(double) m_submaps.Count*100d);
                        }
                    }
                });


            using (IRedisClient client = m_clientManager.GetClient())
            {
                progression.UpdateValue(0, "Storing informations on Redis server...");

                IRedisTypedClient<SubMapBinder> typedClient1 = client.As<SubMapBinder>();
                typedClient1.SetRangeInHash(typedClient1.GetHash<long>(REDIS_KEY), m_submaps.Values.SelectMany(x => x).ToDictionary(x => x.SubMap.GlobalId, x => x.SubMap));
                progression.UpdateValue(50);

                IRedisTypedClient<long[]> typedClient2 = client.As<long[]>();
                typedClient2.SetRangeInHash(typedClient2.GetHash<int>(REDIS_MAPS), m_submaps.ToDictionary(x => x.Key, x => x.Value.Select(y => y.SubMap.GlobalId).ToArray()));
                progression.UpdateValue(100);

                client.Set(REDIS_VERSION, VERSION);
            }

            m_submaps.Clear();
            m_mapsPosition.Clear();

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

        private int? TryGetMapNeighbour(int mapid, MapNeighbour neighbour)
        {
            MapPositionData position;
            if (!m_mapsPosition.TryGetValue(mapid, out position))
                return null;

            switch (neighbour)
            {
                case MapNeighbour.Left:
                    return position.LeftNeighbourId;
                case MapNeighbour.Right:
                    return position.RightNeighbourId;
                case MapNeighbour.Bottom:
                    return position.BottomNeighbourId;
                case MapNeighbour.Top:
                    return position.TopNeighbourId;
            }

            return null;
        }
    }
}