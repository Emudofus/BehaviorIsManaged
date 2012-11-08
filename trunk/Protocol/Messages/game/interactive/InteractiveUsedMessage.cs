#region License GNU GPL
// InteractiveUsedMessage.cs
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
    public class InteractiveUsedMessage : NetworkMessage
    {
        public const uint Id = 5745;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int entityId;
        public int elemId;
        public short skillId;
        public short duration;
        
        public InteractiveUsedMessage()
        {
        }
        
        public InteractiveUsedMessage(int entityId, int elemId, short skillId, short duration)
        {
            this.entityId = entityId;
            this.elemId = elemId;
            this.skillId = skillId;
            this.duration = duration;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteInt(elemId);
            writer.WriteShort(skillId);
            writer.WriteShort(duration);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            entityId = reader.ReadInt();
            if (entityId < 0)
                throw new Exception("Forbidden value on entityId = " + entityId + ", it doesn't respect the following condition : entityId < 0");
            elemId = reader.ReadInt();
            if (elemId < 0)
                throw new Exception("Forbidden value on elemId = " + elemId + ", it doesn't respect the following condition : elemId < 0");
            skillId = reader.ReadShort();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
            duration = reader.ReadShort();
            if (duration < 0)
                throw new Exception("Forbidden value on duration = " + duration + ", it doesn't respect the following condition : duration < 0");
        }
        
    }
    
}