

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ContactLookErrorMessage : NetworkMessage
    {
        public const uint Id = 6045;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int requestId;
        
        public ContactLookErrorMessage()
        {
        }
        
        public ContactLookErrorMessage(int requestId)
        {
            this.requestId = requestId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(requestId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
        }
        
    }
    
}