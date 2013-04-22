

// Generated on 04/17/2013 22:30:05
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