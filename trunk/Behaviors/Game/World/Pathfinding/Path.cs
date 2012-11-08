#region License GNU GPL
// Path.cs
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
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Movements;
using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.World.Pathfinding
{
    public class Path
    {
        private Cell[] m_cellsPath;
        private PathElement[] m_compressedPath;

        /// <summary>
        /// Constructs the Path instance from the complete path as a list of cell
        /// </summary>
        /// <param name="map">Current map</param>
        /// <param name="path">Complete path</param>
        public Path(Map map, IEnumerable<Cell> path)
        {
            Map = map;
            m_cellsPath = path.ToArray();
        }

        /// <summary>
        /// Constructs the Path instance from the compressed path as a list of PathElement
        /// </summary>
        /// <param name="map">Current map</param>
        /// <param name="compressedPath">Compressed Path</param>
        private Path(Map map, IEnumerable<PathElement> compressedPath)
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

        public int MPCost
        {
            get { return (int)Start.ManhattanDistanceTo(End); }
        }

        public bool IsEmpty()
        {
            return m_cellsPath.Length <= 1; // if end == start the path is also empty
        }

        public DirectionsEnum GetEndCellDirection()
        {
            if (m_cellsPath.Length <= 1)
                return DirectionsEnum.DIRECTION_EAST;

            if (m_compressedPath != null)
                return m_compressedPath.Last().Direction;

            return m_cellsPath[m_cellsPath.Length - 2].OrientationToAdjacent(m_cellsPath[m_cellsPath.Length - 1]);
        }

        public PathElement[] GetCompressedPath()
        {
            return m_compressedPath ?? (m_compressedPath = BuildCompressedPath());
        }

        public bool Contains(short cellId)
        {
            return m_cellsPath.Any(entry => entry.Id == cellId);
        }

        /// <summary>
        /// Build an array of keys that represents the path like a path sent by the server
        /// </summary>
        /// <returns></returns>
        public short[] GetServerPathKeys()
        {
            var compressedPath = GetCompressedPath();

            return compressedPath.Select(entry => entry.Cell.Id).ToArray();
        }

        /// <summary>
        /// Build an array of keys that represents the path like a path sent by the client
        /// </summary>
        /// <returns></returns>
        public short[] GetClientPathKeys()
        {
            var compressedPath = GetCompressedPath();

            return compressedPath.Select(entry => (short)(entry.Cell.Id | ((short)entry.Direction << 12))).ToArray();
        }

        public void CutPath(int index)
        {
            if (index > m_cellsPath.Length - 1)
                return;

            m_cellsPath = m_cellsPath.Take(index).ToArray();
        }

        private PathElement[] BuildCompressedPath()
        {
            if (m_cellsPath.Length <= 0)
                return new PathElement[0];

            // only one cell
            if (m_cellsPath.Length <= 1)
                return new[] { new PathElement(m_cellsPath[0], DirectionsEnum.DIRECTION_EAST) };

            // build the path
            var path = new List<PathElement>();
            for (int i = 1; i < m_cellsPath.Length; i++)
            {
                path.Add(new PathElement(m_cellsPath[i - 1], m_cellsPath[i - 1].OrientationToAdjacent(m_cellsPath[i])));
            }

            path.Add(new PathElement(m_cellsPath[m_cellsPath.Length - 1], path[path.Count - 1].Direction));

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

        /// <summary>
        /// Build a Path instance from the keys sent by the server
        /// </summary>
        /// <returns></returns>
        public static Path BuildFromServerCompressedPath(Map map, IEnumerable<short> keys)
        {
            var cells = keys.Select(entry => map.Cells[entry]).ToArray();
            var compressedPath = new List<PathElement>();

            for (int i = 0; i < cells.Length - 1; i++)
            {
                compressedPath.Add(new PathElement(cells[i], cells[i].OrientationTo(cells[i + 1])));
            }

            compressedPath.Add(new PathElement(cells[cells.Length - 1], DirectionsEnum.DIRECTION_EAST));

            return new Path(map, compressedPath);
        }
         
        /// <summary>
        /// Build a Path instance from the keys sent by the client
        /// </summary>
        /// <param name="map"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static Path BuildFromClientCompressedPath(Map map, IEnumerable<short> keys)
        {
            var path = ( from key in keys
                         let cellId = key & 4095
                         let direction = (DirectionsEnum)( ( key >> 12 ) & 7 )
                         select new PathElement(map.Cells[cellId], direction) );

            return new Path(map, path);
        }

        /// <summary>
        /// Get an empty path with the current cell
        /// </summary>
        /// <param name="map"></param>
        /// <param name="startCell"></param>
        /// <returns></returns>
        public static Path GetEmptyPath(Map map, Cell startCell)
        {
            return new Path(map, new [] { startCell });
        }
    }
}