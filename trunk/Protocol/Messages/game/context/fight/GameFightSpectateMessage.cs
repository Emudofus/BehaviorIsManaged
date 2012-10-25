

// Generated on 10/25/2012 10:42:38
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightSpectateMessage : NetworkMessage
    {
        public const uint Id = 6069;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.FightDispellableEffectExtendedInformations[] effects;
        public Types.GameActionMark[] marks;
        public short gameTurn;
        
        public GameFightSpectateMessage()
        {
        }
        
        public GameFightSpectateMessage(Types.FightDispellableEffectExtendedInformations[] effects, Types.GameActionMark[] marks, short gameTurn)
        {
            this.effects = effects;
            this.marks = marks;
            this.gameTurn = gameTurn;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)effects.Length);
            foreach (var entry in effects)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)marks.Length);
            foreach (var entry in marks)
            {
                 entry.Serialize(writer);
            }
            writer.WriteShort(gameTurn);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            effects = new Types.FightDispellableEffectExtendedInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 effects[i] = new Types.FightDispellableEffectExtendedInformations();
                 effects[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            marks = new Types.GameActionMark[limit];
            for (int i = 0; i < limit; i++)
            {
                 marks[i] = new Types.GameActionMark();
                 marks[i].Deserialize(reader);
            }
            gameTurn = reader.ReadShort();
            if (gameTurn < 0)
                throw new Exception("Forbidden value on gameTurn = " + gameTurn + ", it doesn't respect the following condition : gameTurn < 0");
        }
        
    }
    
}