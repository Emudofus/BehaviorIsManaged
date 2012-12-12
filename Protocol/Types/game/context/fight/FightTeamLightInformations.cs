

// Generated on 12/11/2012 19:44:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightTeamLightInformations : AbstractFightTeamInformations
    {
        public const short Id = 115;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte teamMembersCount;
        
        public FightTeamLightInformations()
        {
        }
        
        public FightTeamLightInformations(sbyte teamId, int leaderId, sbyte teamSide, sbyte teamTypeId, sbyte teamMembersCount)
         : base(teamId, leaderId, teamSide, teamTypeId)
        {
            this.teamMembersCount = teamMembersCount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(teamMembersCount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            teamMembersCount = reader.ReadSByte();
            if (teamMembersCount < 0)
                throw new Exception("Forbidden value on teamMembersCount = " + teamMembersCount + ", it doesn't respect the following condition : teamMembersCount < 0");
        }
        
    }
    
}