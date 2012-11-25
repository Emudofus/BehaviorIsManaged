#region License GNU GPL

// SubMap.cs
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

using System.Collections.Generic;

namespace BiM.Behaviors.Game.World.MapTraveling
{
    public class SubMap : SerializableSubMap
    {
        public SubMap(Map map, Cell[] cells)
        {
            Map = map;
            Cells = cells;
        }

        public Map Map
        {
            get;
            private set;
        }

        public override int MapId
        {
            get { return Map.Id; }
        }

        public override int X
        {
            get { return Map.PosX; }
        }

        public override int Y
        {
            get { return Map.PosY; }
        }

        public Cell[] Cells
        {
            get;
            set;
        }
    }
}