

// Generated on 12/11/2012 19:44:23
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildHouseUpdateInformationMessage : NetworkMessage
    {
        public const uint Id = 6181;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.HouseInformationsForGuild housesInformations;
        
        public GuildHouseUpdateInformationMessage()
        {
        }
        
        public GuildHouseUpdateInformationMessage(Types.HouseInformationsForGuild housesInformations)
        {
            this.housesInformations = housesInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            housesInformations.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            housesInformations = new Types.HouseInformationsForGuild();
            housesInformations.Deserialize(reader);
        }
        
    }
    
}