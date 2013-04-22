

// Generated on 04/17/2013 22:30:03
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ContactLookRequestByIdMessage : ContactLookRequestMessage
    {
        public const uint Id = 5935;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int playerId;
        
        public ContactLookRequestByIdMessage()
        {
        }
        
        public ContactLookRequestByIdMessage(byte requestId, sbyte contactType, int playerId)
         : base(requestId, contactType)
        {
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
        }
        
    }
    
}