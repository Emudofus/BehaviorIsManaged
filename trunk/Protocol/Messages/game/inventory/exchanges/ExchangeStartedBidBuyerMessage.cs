

// Generated on 09/23/2012 22:27:02
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeStartedBidBuyerMessage : NetworkMessage
    {
        public const uint Id = 5904;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.SellerBuyerDescriptor buyerDescriptor;
        
        public ExchangeStartedBidBuyerMessage()
        {
        }
        
        public ExchangeStartedBidBuyerMessage(Types.SellerBuyerDescriptor buyerDescriptor)
        {
            this.buyerDescriptor = buyerDescriptor;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            buyerDescriptor.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            buyerDescriptor = new Types.SellerBuyerDescriptor();
            buyerDescriptor.Deserialize(reader);
        }
        
    }
    
}