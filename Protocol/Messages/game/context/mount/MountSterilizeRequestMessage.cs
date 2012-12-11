

// Generated on 12/11/2012 19:44:17
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MountSterilizeRequestMessage : NetworkMessage
    {
        public const uint Id = 5962;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public MountSterilizeRequestMessage()
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