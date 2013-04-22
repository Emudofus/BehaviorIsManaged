

// Generated on 04/17/2013 22:29:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ServersListMessage : NetworkMessage
    {
        public const uint Id = 30;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GameServerInformations[] servers;
        
        public ServersListMessage()
        {
        }
        
        public ServersListMessage(Types.GameServerInformations[] servers)
        {
            this.servers = servers;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)servers.Length);
            foreach (var entry in servers)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            servers = new Types.GameServerInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 servers[i] = new Types.GameServerInformations();
                 servers[i].Deserialize(reader);
            }
        }
        
    }
    
}