

// Generated on 12/11/2012 19:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeRequestOnShopStockMessage : NetworkMessage
    {
        public const uint Id = 5753;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeRequestOnShopStockMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}