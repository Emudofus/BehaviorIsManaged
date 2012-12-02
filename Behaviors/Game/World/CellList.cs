#region License GNU GPL

// CellList.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BiM.Behaviors.Game.World
{
    public class CellList : IEnumerable<Cell>
    {
        private readonly Cell[] m_cells;
        private readonly Dictionary<Point, Cell> m_cellsByPoint = new Dictionary<Point, Cell>();

        public CellList(Cell[] cells)
        {
            m_cells = cells;
            m_cellsByPoint = cells.ToDictionary(entry => entry.Point);
        }

        public Map Map
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cell this[int id]
        {
            get
            {
                if (id < 0 || id >= m_cells.Length)
                    return null;

                return m_cells[id];
            }
        }

        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cell this[Point point]
        {
            get
            {
                Cell cell;
                if (!m_cellsByPoint.TryGetValue(point, out cell))
                    return null;

                return cell;
            }
        }

        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Cell this[int x, int y]
        {
            get { return this[new Point(x, y)]; }
        }

        /// <summary>
        /// Returns the number of cells
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Count
        {
            get { return m_cells.Length; }
        }

        #region IEnumerable<Cell> Members

        public IEnumerator<Cell> GetEnumerator()
        {
            return m_cells.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}