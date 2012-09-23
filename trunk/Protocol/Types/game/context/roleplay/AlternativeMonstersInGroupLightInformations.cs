

// Generated on 09/23/2012 22:27:09
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class AlternativeMonstersInGroupLightInformations
    {
        public const short Id = 394;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int playerCount;
        public Types.MonsterInGroupLightInformations[] monsters;
        
        public AlternativeMonstersInGroupLightInformations()
        {
        }
        
        public AlternativeMonstersInGroupLightInformations(int playerCount, Types.MonsterInGroupLightInformations[] monsters)
        {
            this.playerCount = playerCount;
            this.monsters = monsters;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(playerCount);
            writer.WriteUShort((ushort)monsters.Length);
            foreach (var entry in monsters)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            playerCount = reader.ReadInt();
            var limit = reader.ReadUShort();
            monsters = new Types.MonsterInGroupLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 monsters[i] = new Types.MonsterInGroupLightInformations();
                 monsters[i].Deserialize(reader);
            }
        }
        
    }
    
}