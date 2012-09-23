

// Generated on 09/23/2012 22:26:43
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ServerStatusUpdateMessage : NetworkMessage
    {
        public const uint Id = 50;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GameServerInformations server;
        
        public ServerStatusUpdateMessage()
        {
        }
        
        public ServerStatusUpdateMessage(Types.GameServerInformations server)
        {
            this.server = server;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            server.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            server = new Types.GameServerInformations();
            server.Deserialize(reader);
        }
        
    }
    
}