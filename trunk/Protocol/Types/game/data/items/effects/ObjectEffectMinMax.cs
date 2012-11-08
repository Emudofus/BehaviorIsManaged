#region License GNU GPL
// ObjectEffectMinMax.cs
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
    public class ObjectEffectMinMax : ObjectEffect
    {
        public const short Id = 82;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short min;
        public short max;
        
        public ObjectEffectMinMax()
        {
        }
        
        public ObjectEffectMinMax(short actionId, short min, short max)
         : base(actionId)
        {
            this.min = min;
            this.max = max;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(min);
            writer.WriteShort(max);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            min = reader.ReadShort();
            if (min < 0)
                throw new Exception("Forbidden value on min = " + min + ", it doesn't respect the following condition : min < 0");
            max = reader.ReadShort();
            if (max < 0)
                throw new Exception("Forbidden value on max = " + max + ", it doesn't respect the following condition : max < 0");
        }
        
    }
    
}