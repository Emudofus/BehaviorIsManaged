

// Generated on 09/23/2012 22:27:05
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class URLOpenMessage : NetworkMessage
    {
        public const uint Id = 6266;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int urlId;
        
        public URLOpenMessage()
        {
        }
        
        public URLOpenMessage(int urlId)
        {
            this.urlId = urlId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(urlId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            urlId = reader.ReadInt();
            if (urlId < 0)
                throw new Exception("Forbidden value on urlId = " + urlId + ", it doesn't respect the following condition : urlId < 0");
        }
        
    }
    
}