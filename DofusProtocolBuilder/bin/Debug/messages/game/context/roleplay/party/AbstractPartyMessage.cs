

// Generated on 10/25/2012 10:42:42
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AbstractPartyMessage : NetworkMessage
    {
        public const uint Id = 6274;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int partyId;
        
        public AbstractPartyMessage()
        {
        }
        
        public AbstractPartyMessage(int partyId)
        {
            this.partyId = partyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(partyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            partyId = reader.ReadInt();
            if (partyId < 0)
                throw new Exception("Forbidden value on partyId = " + partyId + ", it doesn't respect the following condition : partyId < 0");
        }
        
    }
    
}