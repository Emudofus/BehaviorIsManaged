

// Generated on 12/11/2012 19:44:21
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyLeaveRequestMessage : AbstractPartyMessage
    {
        public const uint Id = 5593;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PartyLeaveRequestMessage()
        {
        }
        
        public PartyLeaveRequestMessage(int partyId)
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