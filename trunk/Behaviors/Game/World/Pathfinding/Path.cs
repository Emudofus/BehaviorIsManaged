using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.World.Pathfinding
{
    public class Path
    {
        private Cell[] m_cellsPath;
        private ObjectPosition[] m_compressedPath;
        private ObjectPosition m_endPathPosition;

        public Path(Map map, IEnumerable<Cell> path)
        {
            Map = map;
            m_cellsPath = path.ToArray();
        }

        private Path(Map map, IEnumerable<ObjectPosition> compressedPath)
        {
            Map = map;
            m_compressedPath = compressedPath.ToArray();
            m_cellsPath = BuildCompletePath();
        }

        public Map Map
        {
            get;
            private set;
        }

        public Cell Start
        {
            get { return m_cellsPath[0]; }
        }

        public Cell End
        {
            get { return m_cellsPath[m_cellsPath.Length - 1]; }
        }

        public Cell[] Cells
        {
            get { return m_cellsPath; }
        }

        public ObjectPosition EndPosition
        {
            get { return m_endPathPosition ?? (m_endPathPosition = new ObjectPosition(Map, End, GetEndCellDirection())); }
        }

        public int MPCost
        {
            // not the start cell
            get { return m_cellsPath.Length - 1; }
        }

        public bool IsEmpty()
        {
            return m_cellsPath.Length == 0;
        }

        public DirectionsEnum GetEndCellDirection()
        {
            if (m_cellsPath.Length <= 1)
                return DirectionsEnum.DIRECTION_EAST;

            if (m_compressedPath != null)
                return m_compressedPath.Last().Direction;

            return m_cellsPath[m_cellsPath.Length - 2].OrientationToAdjacent(m_cellsPath[m_cellsPath.Length - 1].Point);
        }

        public ObjectPosition[] GetCompressedPath()
        {
            return m_compressedPath ?? (m_compressedPath = BuildCompressedPath());
        }

        public bool Contains(short cellId)
        {
            return m_cellsPath.Any(entry => entry.Id == cellId);
        }

        public IEnumerable<short> GetServerPathKeys()
        {
            return m_cellsPath.Select(entry => entry.Id);
        }

        public void CutPath(int index)
        {
            if (index > m_cellsPath.Length - 1)
                return;

            m_cellsPath = m_cellsPath.Take(index).ToArray();
            m_endPathPosition = new ObjectPosition(Map, End, GetEndCellDirection());
        }

        private ObjectPosition[] BuildCompressedPath()
        {
            if (m_cellsPath.Length <= 0)
                return new ObjectPosition[0];

            // only one cell
            if (m_cellsPath.Length <= 1)
                return new [] { new ObjectPosition(Map, m_cellsPath[0], DirectionsEnum.DIRECTION_EAST) };

            // build the path
            var path = new List<ObjectPosition>();
            for (int i = 1; i < m_cellsPath.Length; i++)
            {
                path.Add(new ObjectPosition(Map, m_cellsPath[i - 1], m_cellsPath[i - 1].OrientationToAdjacent(m_cellsPath[i].Point)));
            }

            path.Add(new ObjectPosition(Map, m_cellsPath[m_cellsPath.Length - 1], path[path.Count - 1].Direction));

            // compress it
            if (path.Count > 0)
            {
                int i = path.Count - 2; // we don't touch to the last vector
                while (i > 0)
                {
                    if (path[i].Direction == path[i - 1].Direction)
                        path.RemoveAt(i);
                    i--;
                }
            }

            return path.ToArray();
        }

        private Cell[] BuildCompletePath()
        {
            var completePath = new List<Cell>();

            for (int i = 0; i < m_compressedPath.Length - 1; i++)
            {
                completePath.Add(m_compressedPath[i].Cell);

                int l = 0;
                var nextPoint = m_compressedPath[i].Cell;
                while (( nextPoint = nextPoint.GetNearestCellInDirection(m_compressedPath[i].Direction) ) != null &&
                      nextPoint.Id != m_compressedPath[i + 1].Cell.Id)
                {
                    if (l > Map.Height * 2 + Map.Width)
                        throw new Exception("Path too long. Maybe an orientation problem ?");

                    completePath.Add(Map.Cells[nextPoint.Id]);

                    l++;
                }
            }

            completePath.Add(m_compressedPath[m_compressedPath.Length - 1].Cell);

            return completePath.ToArray();
        }

        public static Path BuildFromCompressedPath(Map map, IEnumerable<short> keys)
        {
            var path = (from key in keys
                        let cellId = key & 4095
                        let direction = (DirectionsEnum) ((key >> 12) & 7)
                        select new ObjectPosition(map, map.Cells[cellId], direction));

            return new Path(map, path);
        }

        public static Path GetEmptyPath(Map map, Cell startCell)
        {
            return new Path(map, new [] { startCell });
        }
    }
}