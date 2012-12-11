

// Generated on 12/11/2012 19:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CharacterSelectionWithRelookMessage : CharacterSelectionMessage
    {
        public const uint Id = 6353;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int cosmeticId;
        
        public CharacterSelectionWithRelookMessage()
        {
        }
        
        public CharacterSelectionWithRelookMessage(int id, int cosmeticId)
         : base(id)
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