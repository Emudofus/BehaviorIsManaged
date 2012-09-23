

// Generated on 09/23/2012 22:27:08
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightTeamInformations : AbstractFightTeamInformations
    {
        public const short Id = 33;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.FightTeamMemberInformations[] teamMembers;
        
        public FightTeamInformations()
        {
        }
        
        public FightTeamInformations(sbyte teamId, int leaderId, sbyte teamSide, sbyte teamTypeId, Types.FightTeamMemberInformations[] teamMembers)
         : base(teamId, leaderId, teamSide, teamTypeId)
        {
            this.teamMembers = teamMembers;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)teamMembers.Length);
            foreach (var entry in teamMembers)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            teamMembers = new Types.FightTeamMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 teamMembers[i] = Types.ProtocolTypeManager.GetInstance<Types.FightTeamMemberInformations>(reader.ReadShort());
                 teamMembers[i].Deserialize(reader);
            }
        }
        
    }
    
}