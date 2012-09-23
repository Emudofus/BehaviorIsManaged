

// Generated on 09/23/2012 22:27:06
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class SubscriptionZoneMessage : NetworkMessage
    {
        public const uint Id = 5573;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool active;
        
        public SubscriptionZoneMessage()
        {
        }
        
        public SubscriptionZoneMessage(bool active)
        {
            this.active = active;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(active);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            active = reader.ReadBoolean();
        }
        
    }
    
}