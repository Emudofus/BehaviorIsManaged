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
    public class SerializableSubMap
    {
        private int m_mapId;
        private byte m_subMapId;
        private int m_x;
        private int m_y;

        public SerializableSubMap()
        {
            Neighbours = new List<SubMapNeighbour>();
        }

        public SerializableSubMap(int mapId, byte subMapId, int x, int y, List<SubMapNeighbour> neighbours)
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

        public virtual int MapId
        {
            get { return m_mapId; }
            set { m_mapId = value; }
        }

        public virtual byte SubMapId
        {
            get { return m_subMapId; }
            set { m_subMapId = value; }
        }

        public virtual int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public virtual int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// GlobalID of reachable sub maps
        /// </summary>
        public List<SubMapNeighbour> Neighbours
        {
            get;
            private set;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(MapId);
            writer.Write(SubMapId);
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Neighbours.Count);
            foreach (SubMapNeighbour neighbour in Neighbours)
            {
                writer.Write(neighbour.GlobalId);
                TransitionsManager.Instance.SerializeTransition(writer, neighbour.Transition);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            MapId = reader.ReadInt32();
            SubMapId = reader.ReadByte();
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            Neighbours = new List<SubMapNeighbour>();
            int length = reader.ReadInt32();
            for (int i = 0; i < length; i++)
            {
                long id = reader.ReadInt64();
                SubMapTransition transition = TransitionsManager.Instance.DeserializeTransition(reader);
                Neighbours[i] = new SubMapNeighbour(id, transition);
            }
        }
    }
}