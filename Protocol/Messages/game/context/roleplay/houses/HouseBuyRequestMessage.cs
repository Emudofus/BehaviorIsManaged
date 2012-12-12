

// Generated on 12/11/2012 19:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class HouseBuyRequestMessage : NetworkMessage
    {
        public const uint Id = 5738;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int proposedPrice;
        
        public HouseBuyRequestMessage()
        {
        }
        
        public HouseBuyRequestMessage(int proposedPrice)
        {
            this.proposedPrice = proposedPrice;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(proposedPrice);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            proposedPrice = reader.ReadInt();
            if (proposedPrice < 0)
                throw new Exception("Forbidden value on proposedPrice = " + proposedPrice + ", it doesn't respect the following condition : proposedPrice < 0");
        }
        
    }
    
}