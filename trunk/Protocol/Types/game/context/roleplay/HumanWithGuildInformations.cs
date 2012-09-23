

// Generated on 09/23/2012 22:27:09
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class HumanWithGuildInformations : HumanInformations
    {
        public const short Id = 153;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.GuildInformations guildInformations;
        
        public HumanWithGuildInformations()
        {
        }
        
        public HumanWithGuildInformations(Types.EntityLook[] followingCharactersLook, sbyte emoteId, double emoteStartTime, Types.ActorRestrictionsInformations restrictions, short titleId, string titleParam, Types.GuildInformations guildInformations)
         : base(followingCharactersLook, emoteId, emoteStartTime, restrictions, titleId, titleParam)
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