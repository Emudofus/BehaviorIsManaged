#region License GNU GPL
// FightTeamLightInformations.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
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