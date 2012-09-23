

// Generated on 09/23/2012 22:26:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildInformationsMembersMessage : NetworkMessage
    {
        public const uint Id = 5558;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GuildMember[] members;
        
        public GuildInformationsMembersMessage()
        {
        }
        
        public GuildInformationsMembersMessage(Types.GuildMember[] members)
        {
            this.members = members;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)members.Length);
            foreach (var entry in members)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            members = new Types.GuildMember[limit];
            for (int i = 0; i < limit; i++)
            {
                 members[i] = new Types.GuildMember();
                 members[i].Deserialize(reader);
            }
        }
        
    }
    
}