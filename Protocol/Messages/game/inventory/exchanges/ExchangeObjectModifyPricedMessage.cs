

// Generated on 04/17/2013 22:29:57
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeObjectModifyPricedMessage : ExchangeObjectMovePricedMessage
    {
        public const uint Id = 6238;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeObjectModifyPricedMessage()
        {
        }
        
        public ExchangeObjectModifyPricedMessage(int objectUID, int quantity, int price)
         : base(objectUID, quantity, price)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}