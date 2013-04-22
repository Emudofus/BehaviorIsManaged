

// Generated on 04/17/2013 22:29:53
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildMemberWarnOnConnectionStateMessage : NetworkMessage
    {
        public const uint Id = 6160;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        
        public GuildMemberWarnOnConnectionStateMessage()
        {
        }
        
        public GuildMemberWarnOnConnectionStateMessage(bool enable)
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