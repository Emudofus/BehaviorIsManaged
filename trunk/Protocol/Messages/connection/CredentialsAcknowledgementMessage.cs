

// Generated on 09/23/2012 22:26:43
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CredentialsAcknowledgementMessage : NetworkMessage
    {
        public const uint Id = 6314;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public CredentialsAcknowledgementMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}