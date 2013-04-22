

// Generated on 04/17/2013 22:29:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ServerSelectionMessage : NetworkMessage
    {
        public const uint Id = 40;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short serverId;
        
        public ServerSelectionMessage()
        {
        }
        
        public ServerSelectionMessage(short serverId)
        {
            this.serverId = serverId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(serverId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            serverId = reader.ReadShort();
        }
        
    }
    
}