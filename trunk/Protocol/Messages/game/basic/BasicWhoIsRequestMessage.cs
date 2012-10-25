

// Generated on 10/25/2012 10:42:35
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class BasicWhoIsRequestMessage : NetworkMessage
    {
        public const uint Id = 181;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string search;
        
        public BasicWhoIsRequestMessage()
        {
        }
        
        public BasicWhoIsRequestMessage(string search)
        {
            this.search = search;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(search);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            search = reader.ReadUTF();
        }
        
    }
    
}