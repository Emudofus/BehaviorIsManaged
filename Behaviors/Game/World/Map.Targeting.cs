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
using System.Text;

namespace BiM.Behaviors.Game.World
{
    public partial class Map
    {
        /// <summary>
        /// Check if distance from C to the segment [AB] is less or very close to sqrt(2)/2 "units" 
        /// and the projection of C on the line (AB) is inside the segment [AB].
        /// This should give a conservative way to compute LOS. In very close cases 
        /// (where the exact implementation of the LOS algorithm could make the difference), then 
        /// we consider that the LOS is blocked. The safe way. 
        /// </summary>
        private bool TooCloseFromSegment(int cx, int cy, int ax, int ay, int bx, int by)
        {
            const double MIN_DISTANCE_SQUARED = 0.501;
            
            // Distance computing is inspired by Philip Nicoletti algorithm - http://forums.codeguru.com/printthread.php?t=194400&pp=15&page=2     
            int Numerator = (cx - ax) * (bx - ax) + (cy - ay) * (by - ay);
            int Denomenator = (bx - ax) * (bx - ax) + (by - ay) * (by - ay);
            if (Numerator > Denomenator || Numerator < 0) return false; //The point is outside the segment, so it doesn't block the LOS
            double Base = ((ay - cy) * (bx - ax) - (ax - cx) * (by - ay));
            double distanceLineSquared = Base * Base / Denomenator;
            return (distanceLineSquared <= MIN_DISTANCE_SQUARED); // if distance to line is frankly over sqrt(2)/2, it won't block LOS. 
        }

        /// <summary>
        /// Says if Cell1 can see Cell2
        /// If not sure, then returns false. 
        /// </summary>
        /// <param name="Cell1"></param>
        /// <param name="Cell2"></param>
        /// <returns></returns>
        public bool CanBeSeen(Cell cell1, Cell cell2)
        {
            if (cell1 == null || cell2 == null) return false;
            if (cell1 == cell2) return true;
            
            int x1, x2;
            if (cell1.X < cell2.X)
                { x1 = cell1.X; x2 = cell2.X; }
            else
                { x1 = cell2.X; x2 = cell1.X; }
            
            int y1, y2;
            if (cell1.Y < cell2.Y)
                { y1 = cell1.Y; y2 = cell2.Y; }
            else
                { y1 = cell2.Y; y2 = cell1.Y; }
            
            int CellIdStart, CellIdEnd;
            if (cell1.Id < cell2.Id)
                { CellIdStart = cell1.Id; CellIdEnd = cell2.Id; }
            else
                { CellIdStart = cell2.Id; CellIdEnd = cell1.Id; }
            
            Cell info;            
            for (int CellId = CellIdStart + 1; CellId < CellIdEnd; CellId++)
            {
                info = Cells[CellId];
                if (info == null) return false;
                if (!info.LineOfSight && info.X >= x1 && info.X <= x2 && info.Y >= y1 && info.Y <= y2)
                    // If one obstacle on the LOS, returns false
                    if (TooCloseFromSegment(info.X, info.Y, cell1.X, cell1.Y, cell2.X, cell2.Y))
                        return false;
            }
            return true;
        }
    }
}
