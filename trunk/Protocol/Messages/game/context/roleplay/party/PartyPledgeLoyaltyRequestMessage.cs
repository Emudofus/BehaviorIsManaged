

// Generated on 09/23/2012 22:26:55
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyPledgeLoyaltyRequestMessage : AbstractPartyMessage
    {
        public const uint Id = 6269;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool loyal;
        
        public PartyPledgeLoyaltyRequestMessage()
        {
        }
        
        public PartyPledgeLoyaltyRequestMessage(int partyId, bool loyal)
         : base(partyId)
        {
            this.loyal = loyal;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(loyal);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            loyal = reader.ReadBoolean();
        }
        
    }
    
}