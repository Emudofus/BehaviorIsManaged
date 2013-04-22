

// Generated on 04/17/2013 22:30:06
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
        
        public bool hasFriend;
        public bool hasGuildMember;
        public bool hasGroupMember;
        public bool hasMyTaxCollector;
        public sbyte teamMembersCount;
        public int meanLevel;
        
        public FightTeamLightInformations()
        {
        }
        
        public FightTeamLightInformations(sbyte teamId, int leaderId, sbyte teamSide, sbyte teamTypeId, bool hasFriend, bool hasGuildMember, bool hasGroupMember, bool hasMyTaxCollector, sbyte teamMembersCount, int meanLevel)
         : base(teamId, leaderId, teamSide, teamTypeId)
        {
            this.hasFriend = hasFriend;
            this.hasGuildMember = hasGuildMember;
            this.hasGroupMember = hasGroupMember;
            this.hasMyTaxCollector = hasMyTaxCollector;
            this.teamMembersCount = teamMembersCount;
            this.meanLevel = meanLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, hasFriend);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, hasGuildMember);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, hasGroupMember);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 3, hasMyTaxCollector);
            writer.WriteByte(flag1);
            writer.WriteSByte(teamMembersCount);
            writer.WriteInt(meanLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            byte flag1 = reader.ReadByte();
            hasFriend = BooleanByteWrapper.GetFlag(flag1, 0);
            hasGuildMember = BooleanByteWrapper.GetFlag(flag1, 1);
            hasGroupMember = BooleanByteWrapper.GetFlag(flag1, 2);
            hasMyTaxCollector = BooleanByteWrapper.GetFlag(flag1, 3);
            teamMembersCount = reader.ReadSByte();
            if (teamMembersCount < 0)
                throw new Exception("Forbidden value on teamMembersCount = " + teamMembersCount + ", it doesn't respect the following condition : teamMembersCount < 0");
            meanLevel = reader.ReadInt();
            if (meanLevel < 0)
                throw new Exception("Forbidden value on meanLevel = " + meanLevel + ", it doesn't respect the following condition : meanLevel < 0");
        }
        
    }
    
}