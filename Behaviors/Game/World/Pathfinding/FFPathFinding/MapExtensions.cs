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
using System.Linq;
using BiM.Behaviors.Game.World.Data;

namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
    public static class MapExtensions
    {

        public static int GetNeighbourMapId(this IMap map, MapNeighbour mapNeighbour, bool GetStoredVersion)
        {
            if (GetStoredVersion)
                switch (mapNeighbour)
                {
                    case MapNeighbour.Bottom:
                        return map.BottomNeighbourId;
                    case MapNeighbour.Top:
                        return map.TopNeighbourId;
                    case MapNeighbour.Left:
                        return map.LeftNeighbourId;
                    case MapNeighbour.Right:
                        return map.RightNeighbourId;
                    default: throw new ArgumentOutOfRangeException("MapNeighbour");
                }

            int MapChangeMask = GetMapChangeMask(mapNeighbour);

            // Check if at least one cell allow a transition to the supposed-to-be neighbour
            bool ChangeMapFound = map.Cells.Any(cell => (cell.MapChangeData & MapChangeMask) != 0);
            if (ChangeMapFound)
                return new WorldPoint(map.Id, mapNeighbour).MapID;
            return -1;
        }

        public static ICell GetTransitionCell(this IMap map, MapNeighbour mapNeighbour)
        {
            int MapChangeMask = GetMapChangeMask(mapNeighbour);

            // Check if at least one cell allow a transition to the supposed-to-be neighbour
            return map.Cells.FirstOrDefault(cell => (cell.MapChangeData & MapChangeMask) != 0);
        }

        private static int GetMapChangeMask(MapNeighbour? mapNeighbour)
        {
            switch (mapNeighbour)
            {
                case MapNeighbour.Bottom:
                    return 4;
                case MapNeighbour.Top:
                    return 64;
                case MapNeighbour.Left:
                    return 16;
                case MapNeighbour.Right:
                    return 1;
                case null:
                default:
                    return 1 | 4 | 16 | 64;
            }
        }

        private static Random rnd = new Random();

        /// <summary>
        /// Select a random item within the elements of an IEnumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputSet"></param>
        /// <returns></returns>
        static public T GetRandom<T>(this IEnumerable<T> inputSet)
        {
            return GetRandom(inputSet.ToArray());
        }

        /// <summary>
        /// Select a random item within the elements of an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputSet"></param>
        /// <returns></returns>
        static public T GetRandom<T>(this T[] inputSet)
        {
            if (inputSet.Length == 0) return default(T);
            return inputSet[rnd.Next(inputSet.Length)];
        }

        // Select a random transition for another map. If set, limit the search in a given MapNeighbour. 
        static public Cell GetClosestTransitionCell(this Map map, MapNeighbour MapNeighbour, Cell startingCell)
        {
            int MapChangeMask = GetMapChangeMask(MapNeighbour);
            return map.Cells.Where(cell => (cell.MapChangeData & MapChangeMask) != 0).OrderBy(cell => cell.DistanceTo(startingCell)).FirstOrDefault();
        }


        // Select a random transition for another map. If set, limit the search in a given MapNeighbour. 
        static public Cell GetRandomTransitionCell(this Map map, MapNeighbour? MapNeighbour)
        {
            int MapChangeMask = GetMapChangeMask(MapNeighbour);
            return map.Cells.Where(cell => (cell.MapChangeData & MapChangeMask) != 0).GetRandom();
        }

        // Returns all cells that allow a transition to the map in the given MapNeighbour
        // If MapNeighbour = null, then any MapNeighbour applies
        static public IEnumerable<Cell> GetTransitionCells(this Map map, MapNeighbour? MapNeighbour)
        {
            int MapChangeMask = GetMapChangeMask(MapNeighbour);
            return map.Cells.Where(cell => (cell.MapChangeData & MapChangeMask) != 0);
        }

        // Returns all cells that allow a transition to the map in the given MapNeighbour
        // If MapNeighbour = null, then any MapNeighbour applies
        static public IEnumerable<Cell> GetTransitionCells(IEnumerable<Cell> inputCells, MapNeighbour? MapNeighbour)
        {
            int MapChangeMask = GetMapChangeMask(MapNeighbour);
            return inputCells.Where(cell => (cell.MapChangeData & MapChangeMask) != 0);
        }

    }
}

