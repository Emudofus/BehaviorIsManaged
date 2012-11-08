#region License GNU GPL
// PrismFightDefendersStateMessage.cs
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
    public class PrismFightDefendersStateMessage : NetworkMessage
    {
        public const uint Id = 5899;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fightId;
        public Types.CharacterMinimalPlusLookAndGradeInformations[] mainFighters;
        public Types.CharacterMinimalPlusLookAndGradeInformations[] reserveFighters;
        
        public PrismFightDefendersStateMessage()
        {
        }
        
        public PrismFightDefendersStateMessage(double fightId, Types.CharacterMinimalPlusLookAndGradeInformations[] mainFighters, Types.CharacterMinimalPlusLookAndGradeInformations[] reserveFighters)
        {
            this.fightId = fightId;
            this.mainFighters = mainFighters;
            this.reserveFighters = reserveFighters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            writer.WriteUShort((ushort)mainFighters.Length);
            foreach (var entry in mainFighters)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)reserveFighters.Length);
            foreach (var entry in reserveFighters)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            var limit = reader.ReadUShort();
            mainFighters = new Types.CharacterMinimalPlusLookAndGradeInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 mainFighters[i] = new Types.CharacterMinimalPlusLookAndGradeInformations();
                 mainFighters[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            reserveFighters = new Types.CharacterMinimalPlusLookAndGradeInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 reserveFighters[i] = new Types.CharacterMinimalPlusLookAndGradeInformations();
                 reserveFighters[i].Deserialize(reader);
            }
        }
        
    }
    
}