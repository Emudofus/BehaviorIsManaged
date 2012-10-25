

// Generated on 10/25/2012 10:42:44
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyNewMemberMessage : PartyUpdateMessage
    {
        public const uint Id = 6306;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PartyNewMemberMessage()
        {
        }
        
        public PartyNewMemberMessage(int partyId, Types.PartyMemberInformations memberInformations)
         : base(partyId, memberInformations)
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