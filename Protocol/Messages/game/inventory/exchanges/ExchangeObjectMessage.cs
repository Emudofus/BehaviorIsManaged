

// Generated on 12/11/2012 19:44:26
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeObjectMessage : NetworkMessage
    {
        public const uint Id = 5515;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool remote;
        
        public ExchangeObjectMessage()
        {
        }
        
        public ExchangeObjectMessage(bool remote)
        {
            this.remote = remote;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(remote);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            remote = reader.ReadBoolean();
        }
        
    }
    
}