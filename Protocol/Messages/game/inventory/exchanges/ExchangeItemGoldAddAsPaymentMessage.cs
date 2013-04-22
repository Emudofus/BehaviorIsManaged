

// Generated on 04/17/2013 22:29:57
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeItemGoldAddAsPaymentMessage : NetworkMessage
    {
        public const uint Id = 5770;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte paymentType;
        public int quantity;
        
        public ExchangeItemGoldAddAsPaymentMessage()
        {
        }
        
        public ExchangeItemGoldAddAsPaymentMessage(sbyte paymentType, int quantity)
        {
            this.paymentType = paymentType;
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(paymentType);
            writer.WriteInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            paymentType = reader.ReadSByte();
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}