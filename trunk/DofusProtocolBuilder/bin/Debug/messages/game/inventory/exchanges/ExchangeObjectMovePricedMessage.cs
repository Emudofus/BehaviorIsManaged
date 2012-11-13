

// Generated on 10/25/2012 10:42:49
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeObjectMovePricedMessage : ExchangeObjectMoveMessage
    {
        public const uint Id = 5514;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int price;
        
        public ExchangeObjectMovePricedMessage()
        {
        }
        
        public ExchangeObjectMovePricedMessage(int objectUID, int quantity, int price)
         : base(objectUID, quantity)
        {
            this.price = price;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(price);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            price = reader.ReadInt();
        }
        
    }
    
}