

// Generated on 12/11/2012 19:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class DocumentReadingBeginMessage : NetworkMessage
    {
        public const uint Id = 5675;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short documentId;
        
        public DocumentReadingBeginMessage()
        {
        }
        
        public DocumentReadingBeginMessage(short documentId)
        {
            this.documentId = documentId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(documentId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            documentId = reader.ReadShort();
            if (documentId < 0)
                throw new Exception("Forbidden value on documentId = " + documentId + ", it doesn't respect the following condition : documentId < 0");
        }
        
    }
    
}