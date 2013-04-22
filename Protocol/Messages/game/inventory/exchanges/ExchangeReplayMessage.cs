

// Generated on 04/17/2013 22:29:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeReplayMessage : NetworkMessage
    {
        public const uint Id = 6002;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int count;
        
        public ExchangeReplayMessage()
        {
        }
        
        public ExchangeReplayMessage(int count)
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
        }
        
    }
    
}