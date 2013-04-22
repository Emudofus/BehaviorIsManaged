

// Generated on 04/17/2013 22:29:54
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildInvitationMessage : NetworkMessage
    {
        public const uint Id = 5551;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int targetId;
        
        public GuildInvitationMessage()
        {
        }
        
        public GuildInvitationMessage(int targetId)
        {
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            targetId = reader.ReadInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
        }
        
    }
    
}