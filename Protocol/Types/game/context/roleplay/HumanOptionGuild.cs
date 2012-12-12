

// Generated on 12/11/2012 19:44:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class HumanOptionGuild : HumanOption
    {
        public const short Id = 409;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.GuildInformations guildInformations;
        
        public HumanOptionGuild()
        {
        }
        
        public HumanOptionGuild(Types.GuildInformations guildInformations)
        {
            this.guildInformations = guildInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guildInformations.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildInformations = new Types.GuildInformations();
            guildInformations.Deserialize(reader);
        }
        
    }
    
}