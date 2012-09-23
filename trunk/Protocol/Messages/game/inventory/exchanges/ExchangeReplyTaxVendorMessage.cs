

// Generated on 09/23/2012 22:27:01
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeReplyTaxVendorMessage : NetworkMessage
    {
        public const uint Id = 5787;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectValue;
        public int totalTaxValue;
        
        public ExchangeReplyTaxVendorMessage()
        {
        }
        
        public ExchangeReplyTaxVendorMessage(int objectValue, int totalTaxValue)
        {
            this.objectValue = objectValue;
            this.totalTaxValue = totalTaxValue;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(objectValue);
            writer.WriteInt(totalTaxValue);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectValue = reader.ReadInt();
            if (objectValue < 0)
                throw new Exception("Forbidden value on objectValue = " + objectValue + ", it doesn't respect the following condition : objectValue < 0");
            totalTaxValue = reader.ReadInt();
            if (totalTaxValue < 0)
                throw new Exception("Forbidden value on totalTaxValue = " + totalTaxValue + ", it doesn't respect the following condition : totalTaxValue < 0");
        }
        
    }
    
}