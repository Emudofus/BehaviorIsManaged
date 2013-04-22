

// Generated on 04/17/2013 22:29:54
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildMemberLeavingMessage : NetworkMessage
    {
        public const uint Id = 5923;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool kicked;
        public int memberId;
        
        public GuildMemberLeavingMessage()
        {
        }
        
        public GuildMemberLeavingMessage(bool kicked, int memberId)
        {
            this.kicked = kicked;
            this.memberId = memberId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(kicked);
            writer.WriteInt(memberId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            kicked = reader.ReadBoolean();
            memberId = reader.ReadInt();
        }
        
    }
    
}