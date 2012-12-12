

// Generated on 12/11/2012 19:44:21
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PartyInvitationDungeonRequestMessage : PartyInvitationRequestMessage
    {
        public const uint Id = 6245;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short dungeonId;
        
        public PartyInvitationDungeonRequestMessage()
        {
        }
        
        public PartyInvitationDungeonRequestMessage(string name, short dungeonId)
         : base(name)
        {
            this.dungeonId = dungeonId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(dungeonId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            dungeonId = reader.ReadShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
        }
        
    }
    
}