

// Generated on 04/17/2013 22:29:39
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterReplayWithRelookRequestMessage : CharacterReplayRequestMessage
    {
        public const uint Id = 6354;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int cosmeticId;
        
        public CharacterReplayWithRelookRequestMessage()
        {
        }
        
        public CharacterReplayWithRelookRequestMessage(int characterId, int cosmeticId)
         : base(characterId)
        {
            this.cosmeticId = cosmeticId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(cosmeticId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cosmeticId = reader.ReadInt();
            if (cosmeticId < 0)
                throw new Exception("Forbidden value on cosmeticId = " + cosmeticId + ", it doesn't respect the following condition : cosmeticId < 0");
        }
        
    }
    
}