

// Generated on 10/25/2012 10:42:57
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GuildInformations : BasicGuildInformations
    {
        public const short Id = 127;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.GuildEmblem guildEmblem;
        
        public GuildInformations()
        {
        }
        
        public GuildInformations(int guildId, string guildName, Types.GuildEmblem guildEmblem)
         : base(guildId, guildName)
        {
            this.guildEmblem = guildEmblem;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guildEmblem.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildEmblem = new Types.GuildEmblem();
            guildEmblem.Deserialize(reader);
        }
        
    }
    
}