

// Generated on 04/17/2013 22:30:04
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class RawDataMessage : NetworkMessage
    {
        public const uint Id = 6253;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public RawDataMessage()
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