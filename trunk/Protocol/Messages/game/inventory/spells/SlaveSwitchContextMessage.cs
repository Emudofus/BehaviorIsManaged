

// Generated on 09/23/2012 22:27:04
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class SlaveSwitchContextMessage : NetworkMessage
    {
        public const uint Id = 6214;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int summonerId;
        public int slaveId;
        public Types.SpellItem[] slaveSpells;
        public Types.CharacterCharacteristicsInformations slaveStats;
        
        public SlaveSwitchContextMessage()
        {
        }
        
        public SlaveSwitchContextMessage(int summonerId, int slaveId, Types.SpellItem[] slaveSpells, Types.CharacterCharacteristicsInformations slaveStats)
        {
            this.summonerId = summonerId;
            this.slaveId = slaveId;
            this.slaveSpells = slaveSpells;
            this.slaveStats = slaveStats;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(summonerId);
            writer.WriteInt(slaveId);
            writer.WriteUShort((ushort)slaveSpells.Length);
            foreach (var entry in slaveSpells)
            {
                 entry.Serialize(writer);
            }
            slaveStats.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            summonerId = reader.ReadInt();
            slaveId = reader.ReadInt();
            var limit = reader.ReadUShort();
            slaveSpells = new Types.SpellItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 slaveSpells[i] = new Types.SpellItem();
                 slaveSpells[i].Deserialize(reader);
            }
            slaveStats = new Types.CharacterCharacteristicsInformations();
            slaveStats.Deserialize(reader);
        }
        
    }
    
}