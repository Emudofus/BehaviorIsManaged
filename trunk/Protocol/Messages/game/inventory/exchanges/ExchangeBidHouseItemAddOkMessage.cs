

// Generated on 09/23/2012 22:27:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeBidHouseItemAddOkMessage : NetworkMessage
    {
        public const uint Id = 5945;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItemToSellInBid itemInfo;
        
        public ExchangeBidHouseItemAddOkMessage()
        {
        }
        
        public ExchangeBidHouseItemAddOkMessage(Types.ObjectItemToSellInBid itemInfo)
        {
            this.itemInfo = itemInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            itemInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            itemInfo = new Types.ObjectItemToSellInBid();
            itemInfo.Deserialize(reader);
        }
        
    }
    
}