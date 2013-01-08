#region License GNU GPL
// CellRegionBuilder.cs
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
using System.Drawing;
using System.Linq;
using BiM.Behaviors.Game.World.Data;
using BiM.Behaviors.Game.World.Pathfinding.FFPathFinding;

namespace BiM.Behaviors.Game.World.MapTraveling
{
    /// <summary>
    /// Generate the submaps composing a Map
    /// </summary>
    public class SubMapBuilder : IDisposable
    {
        private class BoundCell<TCell>
            where TCell : ICell
        {
            public BoundCell(TCell cell, BoundCell<TCell>[] neighbors)
            {
                Cell = cell;
                Neighbors = neighbors;
            }

            public TCell Cell
            {
                get;
                set;
            }

            public BoundCell<TCell>[] Neighbors
            {
                get;
                set;
            }
        }

        private List<short> m_treatedCells;

        public SubMapBuilder()
        {
        }


        private IEnumerable<TCell> GetAdjacentCells<TCell>(ICell cell, ICellList<TCell> cellList, Predicate<short?> predicate1, Predicate<TCell> predicate2)
            where TCell : ICell
        {
            Point pos = Cell.GetPointFromCell(cell.Id);
            short? cellId;
            TCell adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X + 1, pos.Y + 0) ) != null &&
                predicate1(cellId) && predicate2(adjacent = cellList[cellId.Value]))
                yield return adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X + 0, pos.Y + 1) ) != null &&
                predicate1(cellId) && predicate2(adjacent = cellList[cellId.Value]))
                yield return adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X - 1, pos.Y + 0) ) != null &&
                predicate1(cellId) && predicate2(adjacent = cellList[cellId.Value]))
                yield return adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X + 0, pos.Y - 1) ) != null &&
                predicate1(cellId) && predicate2(adjacent = cellList[cellId.Value]))
                yield return adjacent;
        }

        /// <summary>
        /// Generate the submaps composing the given Map
        /// </summary>
        /// <returns>The submap composing the given Map</returns>
        public SubMap[] GenerateSubMaps(Map map)
        {
            var availableCells = map.Cells.Where(x => x.Walkable).ToArray();

            var results = new List<SubMap>();
            m_treatedCells = new List<short>();

            if (availableCells.Length == 0)
                return new SubMap[0];

            byte submapid = 0;
            while (m_treatedCells.Count < availableCells.Length)
            {
                var cell = availableCells.First(x => !m_treatedCells.Contains(x.Id));

                var subMap = GetConnectedCells(map, cell).ToArray();

                results.Add(new SubMap(map, subMap.ToArray(), ++submapid));
            }

            return results.ToArray();
        }

        /// <summary>
        /// Generate the binders of submaps composing a map
        /// </summary>
        /// <returns>The binders of submap composing the given Map</returns>
        public AdjacentSubMap[] GenerateBinders(IMap map)
        {
            var availableCells = map.Cells.Where(x => x.Walkable).ToArray();

            var results = new List<AdjacentSubMap>();
            m_treatedCells = new List<short>();

            if (availableCells.Length == 0)
                return new AdjacentSubMap[0];
            
            byte submapid = 0;
            while (m_treatedCells.Count < availableCells.Length)
            {
                var cell = availableCells.First(x => !m_treatedCells.Contains(x.Id));

                var subMap = GetConnectedCells(map, cell).ToArray();
                var borderCells = subMap.Where(x => x.MapChangeData > 0).Select(x => x).ToArray();

                if (borderCells.Length <= 0)
                    continue;

                results.Add(new AdjacentSubMap(new SubMapBinder(map.Id, ++submapid, map.X, map.Y, new List<SubMapNeighbour>()), borderCells));
            }

            return results.ToArray();
        }

        private bool IsCellWalkable<TCell>(IMap map, TCell cell, TCell previousCell)
            where TCell : ICell
        {
            if (!cell.Walkable)
                return false;

            if (cell.NonWalkableDuringRP)
                return false;

            // compare the floors
            if (map.UsingNewMovementSystem)
            {
                int floorDiff = Math.Abs(cell.Floor) - Math.Abs(previousCell.Floor);

                if (cell.MoveZone != previousCell.MoveZone || 
                    cell.MoveZone == previousCell.MoveZone && cell.MoveZone == 0 && floorDiff > World.Map.ElevationTolerance)
                    return false;
            }

            // todo : LoS

            return true;
        }

        private IEnumerable<TCell> GetConnectedCells<TCell>(IMap map, TCell cell)
            where TCell : ICell
        {
            if (m_treatedCells.Contains(cell.Id))
                yield break;

            m_treatedCells.Add(cell.Id);
            yield return cell;

            foreach (var adjacent in GetAdjacentCells(cell, map.Cells, x => !m_treatedCells.Contains(x.Value), x => IsCellWalkable(map, x, cell)))
            {
                foreach (var connectedCell in GetConnectedCells(map, adjacent))
                {
                    yield return (TCell)connectedCell;
                }
            }
        }

        public void Dispose()
        {
            m_treatedCells.Clear();
            m_treatedCells = null;
        }
    }
}