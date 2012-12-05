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
using BiM.Behaviors.Game.World.Data;

namespace BiM.Behaviors.Game.World.MapTraveling
{
    public class SubMap : SubMap<Cell, Map>
    {
        public SubMap(Map map, Cell[] cells, byte submapId)
            : base(map, cells, submapId)
        {
        }
    }

    public class SubMap<TCell, TMap> : SerializableSubMap
        where TCell : ICell 
        where TMap : IMap
    {
        public SubMap(TMap map, TCell[] cells, byte submapId)
        {
            Map = map;
            Cells = cells;
            SubMapId = submapId;
        }

        public TMap Map
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
            get { return Map.X; }
        }

        public override int Y
        {
            get { return Map.Y; }
        }

        public TCell[] Cells
        {
            get;
            set;
        }
    }
}