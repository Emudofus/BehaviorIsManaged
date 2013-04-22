

// Generated on 04/17/2013 22:29:55
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TeleportBuddiesAnswerMessage : NetworkMessage
    {
        public const uint Id = 6294;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool accept;
        
        public TeleportBuddiesAnswerMessage()
        {
        }
        
        public TeleportBuddiesAnswerMessage(bool accept)
        {
            this.accept = accept;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(accept);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            accept = reader.ReadBoolean();
        }
        
    }
    
}