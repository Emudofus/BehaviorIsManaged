#region License GNU GPL
// CharacterLevelUpInformationMessage.cs
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
    public class CharacterLevelUpInformationMessage : CharacterLevelUpMessage
    {
        public const uint Id = 6076;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public int id;
        public sbyte relationType;
        
        public CharacterLevelUpInformationMessage()
        {
        }
        
        public CharacterLevelUpInformationMessage(byte newLevel, string name, int id, sbyte relationType)
         : base(newLevel)
        {
            this.name = name;
            this.id = id;
            this.relationType = relationType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
            writer.WriteInt(id);
            writer.WriteSByte(relationType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            relationType = reader.ReadSByte();
        }
        
    }
    
}