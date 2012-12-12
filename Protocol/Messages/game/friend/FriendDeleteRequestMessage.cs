

// Generated on 12/11/2012 19:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class FriendDeleteRequestMessage : NetworkMessage
    {
        public const uint Id = 5603;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        
        public FriendDeleteRequestMessage()
        {
        }
        
        public FriendDeleteRequestMessage(string name)
        {
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
        }
        
    }
    
}