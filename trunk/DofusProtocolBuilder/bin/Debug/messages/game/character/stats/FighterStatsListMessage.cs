

// Generated on 10/25/2012 10:42:36
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class FighterStatsListMessage : NetworkMessage
    {
        public const uint Id = 6322;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.CharacterCharacteristicsInformations stats;
        
        public FighterStatsListMessage()
        {
        }
        
        public FighterStatsListMessage(Types.CharacterCharacteristicsInformations stats)
        {
            this.stats = stats;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            stats.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            stats = new Types.CharacterCharacteristicsInformations();
            stats.Deserialize(reader);
        }
        
    }
    
}