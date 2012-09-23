

// Generated on 09/23/2012 22:27:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeIsReadyMessage : NetworkMessage
    {
        public const uint Id = 5509;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int id;
        public bool ready;
        
        public ExchangeIsReadyMessage()
        {
        }
        
        public ExchangeIsReadyMessage(int id, bool ready)
        {
            this.id = id;
            this.ready = ready;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteBoolean(ready);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            ready = reader.ReadBoolean();
        }
        
    }
    
}