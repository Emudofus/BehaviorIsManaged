

// Generated on 12/11/2012 19:44:21
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyMemberRemoveMessage : AbstractPartyEventMessage
    {
        public const uint Id = 5579;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int leavingPlayerId;
        
        public PartyMemberRemoveMessage()
        {
        }
        
        public PartyMemberRemoveMessage(int partyId, int leavingPlayerId)
         : base(partyId)
        {
            this.leavingPlayerId = leavingPlayerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(leavingPlayerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            leavingPlayerId = reader.ReadInt();
            if (leavingPlayerId < 0)
                throw new Exception("Forbidden value on leavingPlayerId = " + leavingPlayerId + ", it doesn't respect the following condition : leavingPlayerId < 0");
        }
        
    }
    
}