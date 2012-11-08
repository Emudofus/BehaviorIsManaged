#region License GNU GPL
// LivingObjectChangeSkinRequestMessage.cs
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
    public class LivingObjectChangeSkinRequestMessage : NetworkMessage
    {
        public const uint Id = 5725;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int livingUID;
        public byte livingPosition;
        public int skinId;
        
        public LivingObjectChangeSkinRequestMessage()
        {
        }
        
        public LivingObjectChangeSkinRequestMessage(int livingUID, byte livingPosition, int skinId)
        {
            this.livingUID = livingUID;
            this.livingPosition = livingPosition;
            this.skinId = skinId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(livingUID);
            writer.WriteByte(livingPosition);
            writer.WriteInt(skinId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            livingUID = reader.ReadInt();
            if (livingUID < 0)
                throw new Exception("Forbidden value on livingUID = " + livingUID + ", it doesn't respect the following condition : livingUID < 0");
            livingPosition = reader.ReadByte();
            if (livingPosition < 0 || livingPosition > 255)
                throw new Exception("Forbidden value on livingPosition = " + livingPosition + ", it doesn't respect the following condition : livingPosition < 0 || livingPosition > 255");
            skinId = reader.ReadInt();
            if (skinId < 0)
                throw new Exception("Forbidden value on skinId = " + skinId + ", it doesn't respect the following condition : skinId < 0");
        }
        
    }
    
}