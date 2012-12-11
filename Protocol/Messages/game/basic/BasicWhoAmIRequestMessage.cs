

// Generated on 12/11/2012 19:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class BasicWhoAmIRequestMessage : NetworkMessage
    {
        public const uint Id = 5664;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public BasicWhoAmIRequestMessage()
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