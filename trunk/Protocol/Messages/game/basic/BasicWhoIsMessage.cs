#region License GNU GPL
// BasicWhoIsMessage.cs
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
    public class BasicWhoIsMessage : NetworkMessage
    {
        public const uint Id = 180;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool self;
        public sbyte position;
        public string accountNickname;
        public string characterName;
        public short areaId;
        
        public BasicWhoIsMessage()
        {
        }
        
        public BasicWhoIsMessage(bool self, sbyte position, string accountNickname, string characterName, short areaId)
        {
            this.self = self;
            this.position = position;
            this.accountNickname = accountNickname;
            this.characterName = characterName;
            this.areaId = areaId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(self);
            writer.WriteSByte(position);
            writer.WriteUTF(accountNickname);
            writer.WriteUTF(characterName);
            writer.WriteShort(areaId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            self = reader.ReadBoolean();
            position = reader.ReadSByte();
            accountNickname = reader.ReadUTF();
            characterName = reader.ReadUTF();
            areaId = reader.ReadShort();
        }
        
    }
    
}