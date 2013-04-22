

// Generated on 04/17/2013 22:29:37
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AtlasPointInformationsMessage : NetworkMessage
    {
        public const uint Id = 5956;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.AtlasPointsInformations type;
        
        public AtlasPointInformationsMessage()
        {
        }
        
        public AtlasPointInformationsMessage(Types.AtlasPointsInformations type)
        {
            this.type = type;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            type.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            type = new Types.AtlasPointsInformations();
            type.Deserialize(reader);
        }
        
    }
    
}