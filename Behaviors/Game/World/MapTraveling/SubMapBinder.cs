#region License GNU GPL

// SerializableSubMap.cs
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
using System.IO;
using BiM.Behaviors.Game.World.MapTraveling.Transitions;

namespace BiM.Behaviors.Game.World.MapTraveling
{
    /// <summary>
    /// Represent the binding between a submap and his neighbours submaps
    /// </summary>
    public class SubMapBinder
    {
        private int m_mapId;
        private byte m_subMapId;
        private int m_x;
        private int m_y;

        public SubMapBinder()
        {
            Neighbours = new List<SubMapNeighbour>();
        }

        public SubMapBinder(int mapId, byte subMapId, int x, int y, List<SubMapNeighbour> neighbours)
        {
            m_mapId = mapId;
            m_subMapId = subMapId;
            m_x = x;
            m_y = y;
            Neighbours = neighbours;
        }

        public long GlobalId
        {
            get { return (long)MapId << 8 | SubMapId; }
        }

        public int MapId
        {
            get { return m_mapId; }
            set { m_mapId = value; }
        }

        public byte SubMapId
        {
            get { return m_subMapId; }
            set { m_subMapId = value; }
        }

        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// Reachable sub maps
        /// </summary>
        public List<SubMapNeighbour> Neighbours
        {
            get;
            protected set;
        }
    }
}