

// Generated on 09/23/2012 22:27:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeBidHouseItemRemoveOkMessage : NetworkMessage
    {
        public const uint Id = 5946;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int sellerId;
        
        public ExchangeBidHouseItemRemoveOkMessage()
        {
        }
        
        public ExchangeBidHouseItemRemoveOkMessage(int sellerId)
        {
            this.sellerId = sellerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(sellerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            sellerId = reader.ReadInt();
        }
        
    }
    
}