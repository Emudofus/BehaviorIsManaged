

// Generated on 12/11/2012 19:44:34
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class PartyMemberArenaInformations : PartyMemberInformations
    {
        public const short Id = 391;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short rank;
        
        public PartyMemberArenaInformations()
        {
        }
        
        public PartyMemberArenaInformations(int id, byte level, string name, Types.EntityLook entityLook, sbyte breed, bool sex, int lifePoints, int maxLifePoints, short prospecting, byte regenRate, short initiative, bool pvpEnabled, sbyte alignmentSide, short worldX, short worldY, int mapId, short subAreaId, short rank)
         : base(id, level, name, entityLook, breed, sex, lifePoints, maxLifePoints, prospecting, regenRate, initiative, pvpEnabled, alignmentSide, worldX, worldY, mapId, subAreaId)
        {
            this.rank = rank;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(rank);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            rank = reader.ReadShort();
            if (rank < 0 || rank > 2300)
                throw new Exception("Forbidden value on rank = " + rank + ", it doesn't respect the following condition : rank < 0 || rank > 2300");
        }
        
    }
    
}