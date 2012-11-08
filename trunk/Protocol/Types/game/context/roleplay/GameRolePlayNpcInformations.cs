#region License GNU GPL
// GameRolePlayNpcInformations.cs
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
    public class GameRolePlayNpcInformations : GameRolePlayActorInformations
    {
        public const short Id = 156;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short npcId;
        public bool sex;
        public short specialArtworkId;
        
        public GameRolePlayNpcInformations()
        {
        }
        
        public GameRolePlayNpcInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, short npcId, bool sex, short specialArtworkId)
         : base(contextualId, look, disposition)
        {
            this.npcId = npcId;
            this.sex = sex;
            this.specialArtworkId = specialArtworkId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(npcId);
            writer.WriteBoolean(sex);
            writer.WriteShort(specialArtworkId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            npcId = reader.ReadShort();
            if (npcId < 0)
                throw new Exception("Forbidden value on npcId = " + npcId + ", it doesn't respect the following condition : npcId < 0");
            sex = reader.ReadBoolean();
            specialArtworkId = reader.ReadShort();
            if (specialArtworkId < 0)
                throw new Exception("Forbidden value on specialArtworkId = " + specialArtworkId + ", it doesn't respect the following condition : specialArtworkId < 0");
        }
        
    }
    
}