

// Generated on 04/17/2013 22:29:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeReadyMessage : NetworkMessage
    {
        public const uint Id = 5511;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool ready;
        public short step;
        
        public ExchangeReadyMessage()
        {
        }
        
        public ExchangeReadyMessage(bool ready, short step)
        {
            this.ready = ready;
            this.step = step;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(ready);
            writer.WriteShort(step);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            ready = reader.ReadBoolean();
            step = reader.ReadShort();
            if (step < 0)
                throw new Exception("Forbidden value on step = " + step + ", it doesn't respect the following condition : step < 0");
        }
        
    }
    
}