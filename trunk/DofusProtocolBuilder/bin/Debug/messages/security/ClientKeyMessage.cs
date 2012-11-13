

// Generated on 10/25/2012 10:42:54
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ClientKeyMessage : NetworkMessage
    {
        public const uint Id = 5607;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string key;
        
        public ClientKeyMessage()
        {
        }
        
        public ClientKeyMessage(string key)
        {
            this.key = key;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(key);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            key = reader.ReadUTF();
        }
        
    }
    
}