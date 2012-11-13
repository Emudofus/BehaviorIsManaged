

// Generated on 10/25/2012 10:42:53
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PrismFightDefendersStateMessage : NetworkMessage
    {
        public const uint Id = 5899;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fightId;
        public Types.CharacterMinimalPlusLookAndGradeInformations[] mainFighters;
        public Types.CharacterMinimalPlusLookAndGradeInformations[] reserveFighters;
        
        public PrismFightDefendersStateMessage()
        {
        }
        
        public PrismFightDefendersStateMessage(double fightId, Types.CharacterMinimalPlusLookAndGradeInformations[] mainFighters, Types.CharacterMinimalPlusLookAndGradeInformations[] reserveFighters)
        {
            this.fightId = fightId;
            this.mainFighters = mainFighters;
            this.reserveFighters = reserveFighters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            writer.WriteUShort((ushort)mainFighters.Length);
            foreach (var entry in mainFighters)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)reserveFighters.Length);
            foreach (var entry in reserveFighters)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            var limit = reader.ReadUShort();
            mainFighters = new Types.CharacterMinimalPlusLookAndGradeInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 mainFighters[i] = new Types.CharacterMinimalPlusLookAndGradeInformations();
                 mainFighters[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            reserveFighters = new Types.CharacterMinimalPlusLookAndGradeInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 reserveFighters[i] = new Types.CharacterMinimalPlusLookAndGradeInformations();
                 reserveFighters[i].Deserialize(reader);
            }
        }
        
    }
    
}