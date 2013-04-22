

// Generated on 04/17/2013 22:29:53
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildHousesInformationMessage : NetworkMessage
    {
        public const uint Id = 5919;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.HouseInformationsForGuild[] housesInformations;
        
        public GuildHousesInformationMessage()
        {
        }
        
        public GuildHousesInformationMessage(Types.HouseInformationsForGuild[] housesInformations)
        {
            this.housesInformations = housesInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)housesInformations.Length);
            foreach (var entry in housesInformations)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            housesInformations = new Types.HouseInformationsForGuild[limit];
            for (int i = 0; i < limit; i++)
            {
                 housesInformations[i] = new Types.HouseInformationsForGuild();
                 housesInformations[i].Deserialize(reader);
            }
        }
        
    }
    
}