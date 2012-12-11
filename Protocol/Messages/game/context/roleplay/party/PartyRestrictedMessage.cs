

// Generated on 12/11/2012 19:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyRestrictedMessage : AbstractPartyMessage
    {
        public const uint Id = 6175;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool restricted;
        
        public PartyRestrictedMessage()
        {
        }
        
        public PartyRestrictedMessage(int partyId, bool restricted)
         : base(partyId)
        {
            this.restricted = restricted;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(restricted);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            restricted = reader.ReadBoolean();
        }
        
    }
    
}