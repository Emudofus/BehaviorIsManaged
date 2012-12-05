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
using BiM.Behaviors.Game.World.MapTraveling.Storage;
using BiM.Behaviors.Game.World.Pathfinding.FFPathFinding;

namespace BiM.Behaviors.Game.World.MapTraveling
{
    public class SubMapBuilder : SubMapBuilder<Cell, Map>
    {
        public SubMapBuilder(Map map)
            : base(map)
        {
        }
    }

    /// <summary>
    /// Generate the submaps composing a Map
    /// </summary>
    public class SubMapBuilder<TCell, TMap> : IDisposable
        where TCell : ICell
        where TMap : IMap
    {
        private class BoundCell
        {
            public BoundCell(TCell cell, BoundCell[] neighbors)
            {
                Cell = cell;
                Neighbors = neighbors;
            }

            public TCell Cell
            {
                get;
                set;
            }

            public BoundCell[] Neighbors
            {
                get;
                set;
            }
        }

        private List<short> m_treatedCells;

        public SubMapBuilder(TMap map)
        {
            Map = map;
        }

        public TMap Map
        {
            get;
            set;
        }

        private IEnumerable<TCell> GetAdjacentCells(ICell cell, Predicate<short?> predicate1, Predicate<TCell> predicate2)
        {
            Point pos = Cell.GetPointFromCell(cell.Id);
            short? cellId;
            TCell adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X + 1, pos.Y + 0) ) != null &&
                predicate1(cellId) && predicate2(adjacent = (TCell)Map.Cells[cellId.Value]))
                yield return adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X + 0, pos.Y + 1) ) != null &&
                predicate1(cellId) && predicate2(adjacent = (TCell)Map.Cells[cellId.Value]))
                yield return adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X - 1, pos.Y + 0) ) != null &&
                predicate1(cellId) && predicate2(adjacent = (TCell)Map.Cells[cellId.Value]))
                yield return adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X + 0, pos.Y - 1) ) != null &&
                predicate1(cellId) && predicate2(adjacent = (TCell)Map.Cells[cellId.Value]))
                yield return adjacent;
            /*if (( cellId = Cell.GetCellFromPoint(pos.X + 1, pos.Y + 1) ) != null &&
                predicate1(cellId) && predicate2(adjacent = (TCell)Map.Cells[cellId.Value]))
                yield return adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X - 1, pos.Y + 1) ) != null &&
                predicate1(cellId) && predicate2(adjacent = (TCell)Map.Cells[cellId.Value]))
                yield return adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X + 1, pos.Y - 1) ) != null &&
                predicate1(cellId) && predicate2(adjacent = (TCell)Map.Cells[cellId.Value]))
                yield return adjacent;
            if (( cellId = Cell.GetCellFromPoint(pos.X - 1, pos.Y - 1) ) != null &&
                predicate1(cellId) && predicate2(adjacent = (TCell)Map.Cells[cellId.Value]))
                yield return adjacent;*/
        }

        /// <summary>
        /// Generate the submaps composing the given Map
        /// </summary>
        /// <returns>The submap composing the given Map</returns>
        public SubMap<TCell, TMap>[] Build()
        {
            var availableCells = Map.Cells.Where(x => x.Walkable).ToArray();

            var results = new List<SubMap<TCell, TMap>>();
            m_treatedCells = new List<short>();

            if (availableCells.Length == 0)
                return new SubMap<TCell, TMap>[0];

            byte submapid = 0;
            while (m_treatedCells.Count < availableCells.Length)
            {
                var cell = availableCells.First(x => !m_treatedCells.Contains(x.Id));

                var subMap = GetConnectedCells((TCell)cell).ToArray();

                results.Add(new SubMap<TCell, TMap>(Map, subMap.ToArray(), ++submapid));
            }

            return results.ToArray();
        }

        /// <summary>
        /// Generate the submaps composing the given Map
        /// </summary>
        /// <returns>The submap composing the given Map</returns>
        public GeneratedSubMap[] BuildLight()
        {
            var availableCells = Map.Cells.Where(x => x.Walkable).ToArray();

            var results = new List<GeneratedSubMap>();
            m_treatedCells = new List<short>();

            if (availableCells.Length == 0)
                return new GeneratedSubMap[0];
            
            byte submapid = 0;
            while (m_treatedCells.Count < availableCells.Length)
            {
                var cell = availableCells.First(x => !m_treatedCells.Contains(x.Id));

                var subMap = GetConnectedCells((TCell)cell).ToArray();
                var borderCells = subMap.Where(x => x.MapChangeData > 0).Select(x => (ICell)x).ToArray();

                if (borderCells.Length <= 0)
                    continue;

                results.Add(new GeneratedSubMap(new SerializableSubMap(Map.Id, ++submapid, Map.X, Map.Y, new List<SubMapNeighbour>()), borderCells));
            }

            return results.ToArray();
        }

        private bool IsCellWalkable(TCell cell, TCell previousCell)
        {
            if (!cell.Walkable)
                return false;

            if (cell.NonWalkableDuringRP)
                return false;

            // compare the floors
            if (Map.UsingNewMovementSystem)
            {
                int floorDiff = Math.Abs(cell.Floor) - Math.Abs(previousCell.Floor);

                if (cell.MoveZone != previousCell.MoveZone || 
                    cell.MoveZone == previousCell.MoveZone && cell.MoveZone == 0 && floorDiff > World.Map.ElevationTolerance)
                    return false;
            }

            // todo : LoS

            return true;
        }

        private IEnumerable<TCell> GetConnectedCells(TCell cell)
        {
            if (m_treatedCells.Contains(cell.Id))
                yield break;

            m_treatedCells.Add(cell.Id);
            yield return cell;

            foreach (var adjacent in GetAdjacentCells(cell, x => !m_treatedCells.Contains(x.Value), x => IsCellWalkable(x, cell)))
            {
                foreach (var connectedCell in GetConnectedCells(adjacent))
                {
                    yield return connectedCell;
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