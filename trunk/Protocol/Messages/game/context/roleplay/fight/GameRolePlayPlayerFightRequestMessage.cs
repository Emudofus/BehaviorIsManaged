#region License GNU GPL
// GameRolePlayPlayerFightRequestMessage.cs
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
    public class GameRolePlayPlayerFightRequestMessage : NetworkMessage
    {
        public const uint Id = 5731;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        public short targetCellId;
        public bool friendly;
        
        public GameRolePlayPlayerFightRequestMessage()
        {
        }
        
        public GameRolePlayPlayerFightRequestMessage(int targetId, short targetCellId, bool friendly)
        {
            this.targetId = targetId;
            this.targetCellId = targetCellId;
            this.friendly = friendly;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(targetId);
            writer.WriteShort(targetCellId);
            writer.WriteBoolean(friendly);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            targetId = reader.ReadInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
            targetCellId = reader.ReadShort();
            if (targetCellId < -1 || targetCellId > 559)
                throw new Exception("Forbidden value on targetCellId = " + targetCellId + ", it doesn't respect the following condition : targetCellId < -1 || targetCellId > 559");
            friendly = reader.ReadBoolean();
        }
        
    }
    
}