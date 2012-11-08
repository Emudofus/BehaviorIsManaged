#region License GNU GPL
// TeleportDestinationsListMessage.cs
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
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TeleportDestinationsListMessage : NetworkMessage
    {
        public const uint Id = 5960;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte teleporterType;
        public int[] mapIds;
        public short[] subAreaIds;
        public short[] costs;
        
        public TeleportDestinationsListMessage()
        {
        }
        
        public TeleportDestinationsListMessage(sbyte teleporterType, int[] mapIds, short[] subAreaIds, short[] costs)
        {
            this.teleporterType = teleporterType;
            this.mapIds = mapIds;
            this.subAreaIds = subAreaIds;
            this.costs = costs;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(teleporterType);
            writer.WriteUShort((ushort)mapIds.Length);
            foreach (var entry in mapIds)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)subAreaIds.Length);
            foreach (var entry in subAreaIds)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)costs.Length);
            foreach (var entry in costs)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            teleporterType = reader.ReadSByte();
            if (teleporterType < 0)
                throw new Exception("Forbidden value on teleporterType = " + teleporterType + ", it doesn't respect the following condition : teleporterType < 0");
            var limit = reader.ReadUShort();
            mapIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 mapIds[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            subAreaIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 subAreaIds[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            costs = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 costs[i] = reader.ReadShort();
            }
        }
        
    }
    
}