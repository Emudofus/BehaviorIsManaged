

// Generated on 04/17/2013 22:29:56
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeClearPaymentForCraftMessage : NetworkMessage
    {
        public const uint Id = 6145;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte paymentType;
        
        public ExchangeClearPaymentForCraftMessage()
        {
        }
        
        public ExchangeClearPaymentForCraftMessage(sbyte paymentType)
        {
            this.paymentType = paymentType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(paymentType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            paymentType = reader.ReadSByte();
        }
        
    }
    
}