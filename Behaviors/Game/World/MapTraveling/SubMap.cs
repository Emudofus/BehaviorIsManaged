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
using System.Collections.ObjectModel;

namespace BiM.Behaviors.Game.World.MapTraveling
{
    public class SubMap
    {
        private bool m_isBound;
        private List<SubMapNeighbour> m_neighbours; 

        public SubMap(Map map, Cell[] cells, byte submapId)
        {
            Map = map;
            Cells = cells;
            SubMapId = submapId;
            m_isBound = false;
        }

        public SubMap(Map map, Cell[] cells, SubMapBinder binder)
        {
            Map = map;
            Cells = cells;
            SubMapId = binder.SubMapId;
            m_neighbours = binder.Neighbours;
        }

        public Map Map
        {
            get;
            private set;
        }

        public long GlobalId
        {
            get { return (long) Map.Id << 8 | SubMapId; }
        }

        public byte SubMapId
        {
            get;
            private set;
        }

        public int X
        {
            get { return Map.X; }
        }

        public int Y
        {
            get { return Map.Y; }
        }


        public Cell[] Cells
        {
            get;
            private set;
        }

        public bool IsBound()
        {
            return m_isBound;
        }

        public ReadOnlyCollection<SubMapNeighbour> GetNeighbours()
        {
            return !m_isBound ? new List<SubMapNeighbour>().AsReadOnly() : m_neighbours.AsReadOnly();
        }
    }
}