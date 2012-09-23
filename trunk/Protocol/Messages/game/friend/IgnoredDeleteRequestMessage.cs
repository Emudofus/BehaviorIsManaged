

// Generated on 09/23/2012 22:26:56
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class IgnoredDeleteRequestMessage : NetworkMessage
    {
        public const uint Id = 5680;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public bool session;
        
        public IgnoredDeleteRequestMessage()
        {
        }
        
        public IgnoredDeleteRequestMessage(string name, bool session)
        {
            this.name = name;
            this.session = session;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
            writer.WriteBoolean(session);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
            session = reader.ReadBoolean();
        }
        
    }
    
}