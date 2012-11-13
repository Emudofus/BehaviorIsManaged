

// Generated on 10/25/2012 10:42:39
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PaddockSellRequestMessage : NetworkMessage
    {
        public const uint Id = 5953;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int price;
        
        public PaddockSellRequestMessage()
        {
        }
        
        public PaddockSellRequestMessage(int price)
        {
            this.price = price;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(price);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            price = reader.ReadInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
        }
        
    }
    
}