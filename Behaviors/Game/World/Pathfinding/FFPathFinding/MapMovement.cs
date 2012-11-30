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

using System.Collections.Generic;
using System.Diagnostics;
using BiM.Protocol.Enums;


namespace BiM.Behaviors.Game.World.Pathfinding.FFPathFinding
{
    public class MapMovement
    {
        CellInfo[] cells;
        public MapMovement(CellInfo[] _cells)
        {
            cells = _cells;
        }
        private DirectionsEnum GetOrientation(short cellStart, short cellDest)
        {
            int dx = cells[cellDest].x - cells[cellStart].x;
            int dy = cells[cellDest].y - cells[cellStart].y;
            if (dx == 0)
                if (dy == 0) { Debug.Assert(false); return DirectionsEnum.DIRECTION_EAST; } // 0,0 - no mouvement :p
                else
                    if (dy < 0) return DirectionsEnum.DIRECTION_SOUTH_WEST; // 0,-1
                    else
                        return DirectionsEnum.DIRECTION_NORTH_EAST; // 0,1
            else
                if (dx < 0)
                    if (dy == 0) return DirectionsEnum.DIRECTION_NORTH_WEST; // -1,0
                    else
                        if (dy < 0) return DirectionsEnum.DIRECTION_WEST; // -1,-1
                        else
                            return DirectionsEnum.DIRECTION_NORTH; // -1,1
                else
                    if (dy == 0) return DirectionsEnum.DIRECTION_SOUTH_EAST; // 1,0
                    else
                        if (dy < 0) return DirectionsEnum.DIRECTION_SOUTH; // 1,-1
                        else
                            return DirectionsEnum.DIRECTION_EAST; // 1,1
        }

        /// <summary>
        /// Give each cell (included the starting cell) of the path as input, and compute packed cell array for GameMapMovementRequestMessage. 
        /// </summary>
        /// <param name="path">Minimum size = 2 (source and dest cells)</param>
        /// <returns></returns>
        public short[] PackPath(short[] path)
        {
            Debug.Assert(path.Length > 1); // At least source and dest cells          
            List<short> PackedPath = new List<short>();
            if (path.Length < 2) return PackedPath.ToArray();
            DirectionsEnum PreviousOrientation = DirectionsEnum.DIRECTION_EAST;
            DirectionsEnum Orientation = DirectionsEnum.DIRECTION_EAST;
            short PreviousCellId = path[0];
            for (short NoCell = 1; NoCell < path.Length; NoCell++)
            {
                short cellid = path[NoCell];
                Debug.Assert(cellid >= 0 && cellid < CellInfo.NB_CELL);

                Orientation = GetOrientation(PreviousCellId, cellid);
                if (NoCell == 1 || (Orientation != PreviousOrientation) || NoCell == (path.Length - 1)) // Odd, but first step is always packed
                {
                    PackedPath.Add((short)((ushort)cellid | ((ushort)Orientation) << 12));
                    PreviousOrientation = Orientation;
                }

                PreviousCellId = cellid;
            }
            return PackedPath.ToArray();
        }
    }
}
