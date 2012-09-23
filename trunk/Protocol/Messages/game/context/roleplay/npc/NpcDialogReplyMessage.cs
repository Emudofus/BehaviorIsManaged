

// Generated on 09/23/2012 22:26:53
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class NpcDialogReplyMessage : NetworkMessage
    {
        public const uint Id = 5616;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short replyId;
        
        public NpcDialogReplyMessage()
        {
        }
        
        public NpcDialogReplyMessage(short replyId)
        {
            this.replyId = replyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(replyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            replyId = reader.ReadShort();
            if (replyId < 0)
                throw new Exception("Forbidden value on replyId = " + replyId + ", it doesn't respect the following condition : replyId < 0");
        }
        
    }
    
}