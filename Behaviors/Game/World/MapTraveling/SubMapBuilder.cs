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

namespace BiM.Behaviors.Game.World.MapTraveling
{
    /// <summary>
    /// Generate the submaps composing a Map
    /// </summary>
    public class SubMapBuilder : IDisposable
    {
        private class BoundCell
        {
            public BoundCell(Cell cell, BoundCell[] neighbors)
            {
                Cell = cell;
                Neighbors = neighbors;
            }

            public Cell Cell
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

        private List<int> m_treatedCells;
        private Dictionary<int, BoundCell> m_availableCells = new Dictionary<int, BoundCell>();
        private bool m_initialized;

        public SubMapBuilder(Map map)
        {
            Map = map;
        }

        public Map Map
        {
            get;
            set;
        }

        private void Initialize()
        {
            var availableCells = Map.Cells.Where(x => x.Walkable).ToArray();

            foreach (var cell in availableCells)
            {
                m_availableCells.Add(cell.Id, new BoundCell(cell, new BoundCell[0]));
            }

            foreach (var cell in availableCells)
            {
                var boundCell = m_availableCells[cell.Id];

                var adjacents = cell.GetAdjacentCells((adjacent) => IsCellWalkable(adjacent, boundCell.Cell)).ToArray();
                boundCell.Neighbors = new BoundCell[adjacents.Length];

                for (int i = 0; i < adjacents.Length; i++)
                {
                    boundCell.Neighbors[i] = m_availableCells[adjacents[i].Id];
                }
            }

            m_initialized = true;
        }

        /// <summary>
        /// Generate the submaps composing the given Map
        /// </summary>
        /// <returns>The submap composing the given Map</returns>
        public SubMap[] Build()
        {
            if (!m_initialized)
                Initialize();

            var results = new List<SubMap>();
            m_treatedCells = new List<int>();

            if (m_availableCells.Count == 0)
                return new SubMap[0];

            while (m_treatedCells.Count < m_availableCells.Count)
            {
                var cell = m_availableCells.First(x => !m_treatedCells.Contains(x.Value.Cell.Id));

                var subMap = GetConnectedCells(cell.Value).ToArray();
                /*var borders = GetSubMapBorderLines(subMap);
                var vertices = GetSubMapVertices(borders);*/

                results.Add(new SubMap(Map, subMap.Select(x => x.Cell).ToArray()));
            }

            return results.ToArray();
        }

        // useless
        /*
        private BoundCell[][] GetSubMapBorderLines(BoundCell[] cells)
        {
            var borderCells = cells.Where(x => x.Neighbors.Length < 4).ToArray();

            if (borderCells.Length < 2)
                return new BoundCell[][] { borderCells };

            var sortedCells = new List<BoundCell>() { borderCells[0], borderCells[0].Neighbors.First(x => x.Neighbors.Length < 4)};
            while(borderCells.Length != sortedCells.Count)
            {
                var last = sortedCells.Last();
                var secondLast = sortedCells[sortedCells.Count - 2];
                var next = last.Neighbors.Single(x => x.Neighbors.Length < 4 && x != secondLast && borderCells.Contains(x));

                sortedCells.Add(next);
            }

            var lines = new List<BoundCell[]>();
            var currentLine = new List<BoundCell> { sortedCells[0], sortedCells[1] };

            foreach (var cell in sortedCells.Skip(1))
            {
                var last = currentLine[currentLine.Count - 1];
                var secondLast = currentLine[currentLine.Count - 2];

                // aligned
                if (AreAligned(last.Cell.Point, secondLast.Cell.Point, cell.Cell.Point))
                {
                    currentLine.Add(cell);
                }
                else
                {
                    lines.Add(currentLine.ToArray());
                    currentLine = new List<BoundCell>() { last, cell };
                }
            }

            lines.Add(currentLine.ToArray());

            return lines.ToArray();
        }

        private BoundCell[] GetSubMapVertices(BoundCell[][] lines)
        {
            return lines.SelectMany(x => new BoundCell[] { x[0], x[x.Length - 1] }).Distinct().ToArray();
        }
        
        private bool AreAligned(Point a, Point b, Point c)
        {
            return ( b.X - a.X ) * ( c.Y - a.Y ) == ( c.X - a.X ) * ( b.Y - a.Y );
        }
        */
        private bool IsCellWalkable(Cell cell, Cell previousCell)
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

        private IEnumerable<BoundCell> GetConnectedCells(BoundCell cell)
        {
            if (m_treatedCells.Contains(cell.Cell.Id))
                yield break;

            m_treatedCells.Add(cell.Cell.Id);
            yield return cell;

            foreach (var adjacent in cell.Neighbors)
            {
                foreach (var connectedCell in GetConnectedCells(adjacent))
                {
                    yield return connectedCell;
                }
            }
        }

        public void Dispose()
        {
            m_availableCells.Clear();
            m_availableCells = null;
            m_treatedCells.Clear();
            m_treatedCells = null;
        }
    }
}