

// Generated on 12/11/2012 19:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class DownloadPartMessage : NetworkMessage
    {
        public const uint Id = 1503;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string id;
        
        public DownloadPartMessage()
        {
        }
        
        public DownloadPartMessage(string id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(id);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadUTF();
        }
        
    }
    
}