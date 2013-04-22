

// Generated on 04/17/2013 22:29:51
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyRefuseInvitationNotificationMessage : AbstractPartyEventMessage
    {
        public const uint Id = 5596;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int guestId;
        
        public PartyRefuseInvitationNotificationMessage()
        {
        }
        
        public PartyRefuseInvitationNotificationMessage(int partyId, int guestId)
         : base(partyId)
        {
            this.guestId = guestId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(guestId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guestId = reader.ReadInt();
            if (guestId < 0)
                throw new Exception("Forbidden value on guestId = " + guestId + ", it doesn't respect the following condition : guestId < 0");
        }
        
    }
    
}