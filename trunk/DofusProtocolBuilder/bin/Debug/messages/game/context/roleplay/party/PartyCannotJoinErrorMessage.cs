

// Generated on 10/25/2012 10:42:43
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyCannotJoinErrorMessage : AbstractPartyMessage
    {
        public const uint Id = 5583;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte reason;
        
        public PartyCannotJoinErrorMessage()
        {
        }
        
        public PartyCannotJoinErrorMessage(int partyId, sbyte reason)
         : base(partyId)
        {
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
        }
        
    }
    
}