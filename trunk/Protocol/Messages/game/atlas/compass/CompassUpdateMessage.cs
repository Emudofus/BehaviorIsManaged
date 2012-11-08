#region License GNU GPL
// CompassUpdateMessage.cs
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
    public class CompassUpdateMessage : NetworkMessage
    {
        public const uint Id = 5591;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte type;
        public short worldX;
        public short worldY;
        
        public CompassUpdateMessage()
        {
        }
        
        public CompassUpdateMessage(sbyte type, short worldX, short worldY)
        {
            this.type = type;
            this.worldX = worldX;
            this.worldY = worldY;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(type);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
        }
        
    }
    
}