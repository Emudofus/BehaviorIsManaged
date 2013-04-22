

// Generated on 04/17/2013 22:29:45
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class EmotePlayRequestMessage : NetworkMessage
    {
        public const uint Id = 5685;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte emoteId;
        
        public EmotePlayRequestMessage()
        {
        }
        
        public EmotePlayRequestMessage(sbyte emoteId)
        {
            this.emoteId = emoteId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(emoteId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            emoteId = reader.ReadSByte();
            if (emoteId < 0)
                throw new Exception("Forbidden value on emoteId = " + emoteId + ", it doesn't respect the following condition : emoteId < 0");
        }
        
    }
    
}