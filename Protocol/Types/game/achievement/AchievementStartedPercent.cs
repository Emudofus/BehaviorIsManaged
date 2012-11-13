#region License GNU GPL
// AchievementStartedPercent.cs
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
    public class AchievementStartedPercent : Achievement
    {
        public const short Id = 362;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte completionPercent;
        
        public AchievementStartedPercent()
        {
        }
        
        public AchievementStartedPercent(short id, sbyte completionPercent)
         : base(id)
        {
            this.completionPercent = completionPercent;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(completionPercent);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            completionPercent = reader.ReadSByte();
            if (completionPercent < 0)
                throw new Exception("Forbidden value on completionPercent = " + completionPercent + ", it doesn't respect the following condition : completionPercent < 0");
        }
        
    }
    
}