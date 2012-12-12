

// Generated on 12/11/2012 19:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class HouseKickRequestMessage : NetworkMessage
    {
        public const uint Id = 5698;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int id;
        
        public HouseKickRequestMessage()
        {
        }
        
        public HouseKickRequestMessage(int id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
        }
        
    }
    
}