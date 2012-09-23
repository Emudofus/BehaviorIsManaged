

// Generated on 09/23/2012 22:27:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeItemAutoCraftRemainingMessage : NetworkMessage
    {
        public const uint Id = 6015;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int count;
        
        public ExchangeItemAutoCraftRemainingMessage()
        {
        }
        
        public ExchangeItemAutoCraftRemainingMessage(int count)
        {
            this.count = count;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(count);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            count = reader.ReadInt();
            if (count < 0)
                throw new Exception("Forbidden value on count = " + count + ", it doesn't respect the following condition : count < 0");
        }
        
    }
    
}