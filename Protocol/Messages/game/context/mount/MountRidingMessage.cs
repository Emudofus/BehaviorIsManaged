

// Generated on 04/17/2013 22:29:44
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MountRidingMessage : NetworkMessage
    {
        public const uint Id = 5967;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool isRiding;
        
        public MountRidingMessage()
        {
        }
        
        public MountRidingMessage(bool isRiding)
        {
            this.isRiding = isRiding;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(isRiding);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            isRiding = reader.ReadBoolean();
        }
        
    }
    
}