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
                if (id <= 0 || id >= m_cells.Length)
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

        public IEnumerator<Cell> GetEnumerator()
        {
            return m_cells.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}