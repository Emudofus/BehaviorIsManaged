

// Generated on 12/11/2012 19:44:20
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyDeletedMessage : AbstractPartyMessage
    {
        public const uint Id = 6261;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PartyDeletedMessage()
        {
        }
        
        public PartyDeletedMessage(int partyId)
         : base(partyId)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}