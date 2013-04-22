

// Generated on 04/17/2013 22:30:04
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TrustStatusMessage : NetworkMessage
    {
        public const uint Id = 6267;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool trusted;
        
        public TrustStatusMessage()
        {
        }
        
        public TrustStatusMessage(bool trusted)
        {
            this.trusted = trusted;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(trusted);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            trusted = reader.ReadBoolean();
        }
        
    }
    
}