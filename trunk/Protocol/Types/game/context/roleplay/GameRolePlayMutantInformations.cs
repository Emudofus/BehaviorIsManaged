#region License GNU GPL
// GameRolePlayMutantInformations.cs
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
    public class GameRolePlayMutantInformations : GameRolePlayHumanoidInformations
    {
        public const short Id = 3;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int monsterId;
        public sbyte powerLevel;
        
        public GameRolePlayMutantInformations()
        {
        }
        
        public GameRolePlayMutantInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name, Types.HumanInformations humanoidInfo, int accountId, int monsterId, sbyte powerLevel)
         : base(contextualId, look, disposition, name, humanoidInfo, accountId)
        {
            this.monsterId = monsterId;
            this.powerLevel = powerLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(monsterId);
            writer.WriteSByte(powerLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            monsterId = reader.ReadInt();
            powerLevel = reader.ReadSByte();
        }
        
    }
    
}