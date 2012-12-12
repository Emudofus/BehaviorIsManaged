

// Generated on 12/11/2012 19:44:35
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class HouseInformationsExtended : HouseInformations
    {
        public const short Id = 112;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.GuildInformations guildInfo;
        
        public HouseInformationsExtended()
        {
        }
        
        public HouseInformationsExtended(bool isOnSale, bool isSaleLocked, int houseId, int[] doorsOnMap, string ownerName, short modelId, Types.GuildInformations guildInfo)
         : base(isOnSale, isSaleLocked, houseId, doorsOnMap, ownerName, modelId)
        {
            this.guildInfo = guildInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guildInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildInfo = new Types.GuildInformations();
            guildInfo.Deserialize(reader);
        }
        
    }
    
}