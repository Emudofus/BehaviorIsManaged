#region License GNU GPL
// CharacterDeletionRequestMessage.cs
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
    public class CharacterDeletionRequestMessage : NetworkMessage
    {
        public const uint Id = 165;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int characterId;
        public string secretAnswerHash;
        
        public CharacterDeletionRequestMessage()
        {
        }
        
        public CharacterDeletionRequestMessage(int characterId, string secretAnswerHash)
        {
            this.characterId = characterId;
            this.secretAnswerHash = secretAnswerHash;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(characterId);
            writer.WriteUTF(secretAnswerHash);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
            secretAnswerHash = reader.ReadUTF();
        }
        
    }
    
}