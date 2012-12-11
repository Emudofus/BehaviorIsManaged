

// Generated on 12/11/2012 19:44:25
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeBidPriceMessage : NetworkMessage
    {
        public const uint Id = 5755;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int genericId;
        public int averagePrice;
        
        public ExchangeBidPriceMessage()
        {
        }
        
        public ExchangeBidPriceMessage(int genericId, int averagePrice)
        {
            this.genericId = genericId;
            this.averagePrice = averagePrice;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(genericId);
            writer.WriteInt(averagePrice);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            genericId = reader.ReadInt();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            averagePrice = reader.ReadInt();
            if (averagePrice < 0)
                throw new Exception("Forbidden value on averagePrice = " + averagePrice + ", it doesn't respect the following condition : averagePrice < 0");
        }
        
    }
    
}