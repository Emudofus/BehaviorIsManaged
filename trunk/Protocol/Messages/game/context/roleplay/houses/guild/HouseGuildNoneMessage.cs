

// Generated on 09/23/2012 22:26:52
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class HouseGuildNoneMessage : NetworkMessage
    {
        public const uint Id = 5701;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short houseId;
        
        public HouseGuildNoneMessage()
        {
        }
        
        public HouseGuildNoneMessage(short houseId)
        {
            this.houseId = houseId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(houseId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            houseId = reader.ReadShort();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
        }
        
    }
    
}