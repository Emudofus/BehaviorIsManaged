

// Generated on 09/23/2012 22:27:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeGoldPaymentForCraftMessage : NetworkMessage
    {
        public const uint Id = 5833;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool onlySuccess;
        public int goldSum;
        
        public ExchangeGoldPaymentForCraftMessage()
        {
        }
        
        public ExchangeGoldPaymentForCraftMessage(bool onlySuccess, int goldSum)
        {
            this.onlySuccess = onlySuccess;
            this.goldSum = goldSum;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(onlySuccess);
            writer.WriteInt(goldSum);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            onlySuccess = reader.ReadBoolean();
            goldSum = reader.ReadInt();
            if (goldSum < 0)
                throw new Exception("Forbidden value on goldSum = " + goldSum + ", it doesn't respect the following condition : goldSum < 0");
        }
        
    }
    
}