#region License GNU GPL
// VillageConquestPrismInformation.cs
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
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class VillageConquestPrismInformation
    {
        public const short Id = 379;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public bool isEntered;
        public bool isInRoom;
        public short areaId;
        public sbyte areaAlignment;
        
        public VillageConquestPrismInformation()
        {
        }
        
        public VillageConquestPrismInformation(bool isEntered, bool isInRoom, short areaId, sbyte areaAlignment)
        {
            this.isEntered = isEntered;
            this.isInRoom = isInRoom;
            this.areaId = areaId;
            this.areaAlignment = areaAlignment;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, isEntered);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, isInRoom);
            writer.WriteByte(flag1);
            writer.WriteShort(areaId);
            writer.WriteSByte(areaAlignment);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            isEntered = BooleanByteWrapper.GetFlag(flag1, 0);
            isInRoom = BooleanByteWrapper.GetFlag(flag1, 1);
            areaId = reader.ReadShort();
            if (areaId < 0)
                throw new Exception("Forbidden value on areaId = " + areaId + ", it doesn't respect the following condition : areaId < 0");
            areaAlignment = reader.ReadSByte();
            if (areaAlignment < 0)
                throw new Exception("Forbidden value on areaAlignment = " + areaAlignment + ", it doesn't respect the following condition : areaAlignment < 0");
        }
        
    }
    
}