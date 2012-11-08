#region License GNU GPL
// ActorAlignmentInformations.cs
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
    public class ActorAlignmentInformations
    {
        public const short Id = 201;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte alignmentSide;
        public sbyte alignmentValue;
        public sbyte alignmentGrade;
        public ushort dishonor;
        public int characterPower;
        
        public ActorAlignmentInformations()
        {
        }
        
        public ActorAlignmentInformations(sbyte alignmentSide, sbyte alignmentValue, sbyte alignmentGrade, ushort dishonor, int characterPower)
        {
            this.alignmentSide = alignmentSide;
            this.alignmentValue = alignmentValue;
            this.alignmentGrade = alignmentGrade;
            this.dishonor = dishonor;
            this.characterPower = characterPower;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(alignmentSide);
            writer.WriteSByte(alignmentValue);
            writer.WriteSByte(alignmentGrade);
            writer.WriteUShort(dishonor);
            writer.WriteInt(characterPower);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            alignmentSide = reader.ReadSByte();
            alignmentValue = reader.ReadSByte();
            if (alignmentValue < 0)
                throw new Exception("Forbidden value on alignmentValue = " + alignmentValue + ", it doesn't respect the following condition : alignmentValue < 0");
            alignmentGrade = reader.ReadSByte();
            if (alignmentGrade < 0)
                throw new Exception("Forbidden value on alignmentGrade = " + alignmentGrade + ", it doesn't respect the following condition : alignmentGrade < 0");
            dishonor = reader.ReadUShort();
            if (dishonor < 0 || dishonor > 500)
                throw new Exception("Forbidden value on dishonor = " + dishonor + ", it doesn't respect the following condition : dishonor < 0 || dishonor > 500");
            characterPower = reader.ReadInt();
            if (characterPower < 0)
                throw new Exception("Forbidden value on characterPower = " + characterPower + ", it doesn't respect the following condition : characterPower < 0");
        }
        
    }
    
}