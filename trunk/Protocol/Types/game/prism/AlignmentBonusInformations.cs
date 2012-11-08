#region License GNU GPL
// AlignmentBonusInformations.cs
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
    public class AlignmentBonusInformations
    {
        public const short Id = 135;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int pctbonus;
        public double grademult;
        
        public AlignmentBonusInformations()
        {
        }
        
        public AlignmentBonusInformations(int pctbonus, double grademult)
        {
            this.pctbonus = pctbonus;
            this.grademult = grademult;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(pctbonus);
            writer.WriteDouble(grademult);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            pctbonus = reader.ReadInt();
            if (pctbonus < 0)
                throw new Exception("Forbidden value on pctbonus = " + pctbonus + ", it doesn't respect the following condition : pctbonus < 0");
            grademult = reader.ReadDouble();
        }
        
    }
    
}