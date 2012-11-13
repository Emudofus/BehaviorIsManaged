

// Generated on 10/25/2012 10:42:41
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class HouseGuildRightsViewMessage : NetworkMessage
    {
        public const uint Id = 5700;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public HouseGuildRightsViewMessage()
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