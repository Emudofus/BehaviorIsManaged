

// Generated on 04/17/2013 22:29:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeReplayCountModifiedMessage : NetworkMessage
    {
        public const uint Id = 6023;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int count;
        
        public ExchangeReplayCountModifiedMessage()
        {
        }
        
        public ExchangeReplayCountModifiedMessage(int count)
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