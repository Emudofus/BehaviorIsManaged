

// Generated on 12/11/2012 19:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AcquaintanceServerListMessage : NetworkMessage
    {
        public const uint Id = 6142;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] servers;
        
        public AcquaintanceServerListMessage()
        {
        }
        
        public AcquaintanceServerListMessage(short[] servers)
        {
            this.servers = servers;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)servers.Length);
            foreach (var entry in servers)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            servers = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 servers[i] = reader.ReadShort();
            }
        }
        
    }
    
}