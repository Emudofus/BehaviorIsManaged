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
        private Tuple<Cell, DirectionsEnum>[] m_compressedPath;
        private ObjectPosition m_endPathPosition;

        public Path(Map map, IEnumerable<Cell> path)
        {
            Map = map;
            m_cellsPath = path.ToArray();
        }

        private Path(Map map, IEnumerable<Tuple<Cell, DirectionsEnum>> compressedPath)
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
                return m_compressedPath.Last().Item2;

            return m_cellsPath[m_cellsPath.Length - 2].OrientationToAdjacent(m_cellsPath[m_cellsPath.Length - 1].Point);
        }

        public Tuple<Cell, DirectionsEnum>[] GetCompressedPath()
        {
            return m_compressedPath ?? (m_compressedPath = BuildCompressedPath());
        }

        public bool Contains(short cellId)
        {
            return m_cellsPath.Any(entry => entry.Id == cellId);
        }

        public short[] GetServerPathKeys()
        {
            return m_cellsPath.Select(entry => entry.Id).ToArray();
        }

        public short[] GetCompressedPathKeys()
        {
            var compressedPath = GetCompressedPath();

            return compressedPath.Select(entry => (short)(entry.Item1.Id | ((short)entry.Item2 << 12))).ToArray();
        }

        public void CutPath(int index)
        {
            if (index > m_cellsPath.Length - 1)
                return;

            m_cellsPath = m_cellsPath.Take(index).ToArray();
            m_endPathPosition = new ObjectPosition(Map, End, GetEndCellDirection());
        }

        private Tuple<Cell, DirectionsEnum>[] BuildCompressedPath()
        {
            if (m_cellsPath.Length <= 0)
                return new Tuple<Cell, DirectionsEnum>[0];

            // only one cell
            if (m_cellsPath.Length <= 1)
                return new[] { new Tuple<Cell, DirectionsEnum>(m_cellsPath[0], DirectionsEnum.DIRECTION_EAST) };

            // build the path
            var path = new List<Tuple<Cell, DirectionsEnum>>();
            for (int i = 1; i < m_cellsPath.Length; i++)
            {
                path.Add(new Tuple<Cell, DirectionsEnum>(m_cellsPath[i - 1], m_cellsPath[i - 1].OrientationToAdjacent(m_cellsPath[i].Point)));
            }

            path.Add(new Tuple<Cell, DirectionsEnum>(m_cellsPath[m_cellsPath.Length - 1], path[path.Count - 1].Item2));

            // compress it
            if (path.Count > 0)
            {
                int i = path.Count - 2; // we don't touch to the last vector
                while (i > 0)
                {
                    if (path[i].Item2 == path[i - 1].Item2)
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
                completePath.Add(m_compressedPath[i].Item1);

                int l = 0;
                var nextPoint = m_compressedPath[i].Item1;
                while (( nextPoint = nextPoint.GetNearestCellInDirection(m_compressedPath[i].Item2) ) != null &&
                      nextPoint.Id != m_compressedPath[i + 1].Item1.Id)
                {
                    if (l > Map.Height * 2 + Map.Width)
                        throw new Exception("Path too long. Maybe an orientation problem ?");

                    completePath.Add(Map.Cells[nextPoint.Id]);

                    l++;
                }
            }

            completePath.Add(m_compressedPath[m_compressedPath.Length - 1].Item1);

            return completePath.ToArray();
        }

        public static Path BuildFromServerCompressedPath(Map map, IEnumerable<short> keys)
        {
            var cells = keys.Select(entry => map.Cells[entry]).ToArray();
            var compressedPath = new List<Tuple<Cell, DirectionsEnum>>();

            for (int i = 0; i < cells.Length - 1; i++)
            {
                compressedPath.Add(Tuple.Create(cells[i], cells[i].OrientationTo(cells[i + 1])));
            }

            compressedPath.Add(Tuple.Create(cells[cells.Length - 1], DirectionsEnum.DIRECTION_EAST));

            return new Path(map, compressedPath);
        }

        public static Path BuildFromClientCompressedPath(Map map, IEnumerable<short> keys)
        {
            var path = ( from key in keys
                         let cellId = key & 4095
                         let direction = (DirectionsEnum)( ( key >> 12 ) & 7 )
                         select Tuple.Create(map.Cells[cellId], direction) );

            return new Path(map, path);
        }

        public static Path GetEmptyPath(Map map, Cell startCell)
        {
            return new Path(map, new [] { startCell });
        }
    }
}