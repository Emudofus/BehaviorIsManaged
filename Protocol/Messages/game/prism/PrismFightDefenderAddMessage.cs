

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PrismFightDefenderAddMessage : NetworkMessage
    {
        public const uint Id = 5895;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fightId;
        public Types.CharacterMinimalPlusLookAndGradeInformations fighterMovementInformations;
        public bool inMain;
        
        public PrismFightDefenderAddMessage()
        {
        }
        
        public PrismFightDefenderAddMessage(double fightId, Types.CharacterMinimalPlusLookAndGradeInformations fighterMovementInformations, bool inMain)
        {
            this.fightId = fightId;
            this.fighterMovementInformations = fighterMovementInformations;
            this.inMain = inMain;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            fighterMovementInformations.Serialize(writer);
            writer.WriteBoolean(inMain);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            fighterMovementInformations = new Types.CharacterMinimalPlusLookAndGradeInformations();
            fighterMovementInformations.Deserialize(reader);
            inMain = reader.ReadBoolean();
        }
        
    }
    
}