#region License GNU GPL
// PrismFightDefenderAddMessage.cs
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
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PrismFightDefenderAddMessage : NetworkMessage
    {
        public const uint Id = 5895;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fightId;
        public Types.CharacterMinimalPlusLookAndGradeInformations fighterMovementInformations;
        public bool inMain;
        
        public PrismFightDefenderAddMessage()
        {
        }
        
        public PrismFightDefenderAddMessage(double fightId, Types.CharacterMinimalPlusLookAndGradeInformations fighterMovementInformations, bool inMain)
        {
            this.fightId = fightId;
            this.fighterMovementInformations = fighterMovementInformations;
            this.inMain = inMain;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            fighterMovementInformations.Serialize(writer);
            writer.WriteBoolean(inMain);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            fighterMovementInformations = new Types.CharacterMinimalPlusLookAndGradeInformations();
            fighterMovementInformations.Deserialize(reader);
            inMain = reader.ReadBoolean();
        }
        
    }
    
}