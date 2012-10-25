

// Generated on 10/25/2012 10:42:40
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class EmoteAddMessage : NetworkMessage
    {
        public const uint Id = 5644;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte emoteId;
        
        public EmoteAddMessage()
        {
        }
        
        public EmoteAddMessage(sbyte emoteId)
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