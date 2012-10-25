

// Generated on 10/25/2012 10:42:45
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class FriendWarnOnConnectionStateMessage : NetworkMessage
    {
        public const uint Id = 5630;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        
        public FriendWarnOnConnectionStateMessage()
        {
        }
        
        public FriendWarnOnConnectionStateMessage(bool enable)
        {
            this.enable = enable;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(enable);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            enable = reader.ReadBoolean();
        }
        
    }
    
}