

// Generated on 12/11/2012 19:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class HouseSellRequestMessage : NetworkMessage
    {
        public const uint Id = 5697;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int amount;
        
        public HouseSellRequestMessage()
        {
        }
        
        public HouseSellRequestMessage(int amount)
        {
            this.amount = amount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(amount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            amount = reader.ReadInt();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
        }
        
    }
    
}