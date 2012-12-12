

// Generated on 12/11/2012 19:44:23
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildInvitationAnswerMessage : NetworkMessage
    {
        public const uint Id = 5556;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool accept;
        
        public GuildInvitationAnswerMessage()
        {
        }
        
        public GuildInvitationAnswerMessage(bool accept)
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