

// Generated on 09/23/2012 22:26:55
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyNewGuestMessage : AbstractPartyEventMessage
    {
        public const uint Id = 6260;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.PartyGuestInformations guest;
        
        public PartyNewGuestMessage()
        {
        }
        
        public PartyNewGuestMessage(int partyId, Types.PartyGuestInformations guest)
         : base(partyId)
        {
            this.guest = guest;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guest.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guest = new Types.PartyGuestInformations();
            guest.Deserialize(reader);
        }
        
    }
    
}