#region License GNU GPL

// PathElement.cs
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

using BiM.Protocol.Enums;

namespace BiM.Behaviors.Game.World.Pathfinding
{
    public class PathElement
    {
        public PathElement(Cell cell, DirectionsEnum direction)
        {
            Cell = cell;
            Direction = direction;
        }

        public Cell Cell { get; set; }
        public DirectionsEnum Direction { get; set; }
    }
}