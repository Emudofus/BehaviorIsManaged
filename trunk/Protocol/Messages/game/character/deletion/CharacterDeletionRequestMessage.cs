

// Generated on 10/25/2012 10:42:36
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