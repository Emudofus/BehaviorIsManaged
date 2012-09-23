

// Generated on 09/23/2012 22:27:06
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class KrosmasterTransferRequestMessage : NetworkMessage
    {
        public const uint Id = 6349;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string uid;
        
        public KrosmasterTransferRequestMessage()
        {
        }
        
        public KrosmasterTransferRequestMessage(string uid)
        {
            this.uid = uid;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(uid);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            uid = reader.ReadUTF();
        }
        
    }
    
}