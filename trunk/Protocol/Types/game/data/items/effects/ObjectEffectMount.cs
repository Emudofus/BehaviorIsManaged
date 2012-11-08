#region License GNU GPL
// ObjectEffectMount.cs
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
    public class ObjectEffectMount : ObjectEffect
    {
        public const short Id = 179;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int mountId;
        public double date;
        public short modelId;
        
        public ObjectEffectMount()
        {
        }
        
        public ObjectEffectMount(short actionId, int mountId, double date, short modelId)
         : base(actionId)
        {
            this.mountId = mountId;
            this.date = date;
            this.modelId = modelId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(mountId);
            writer.WriteDouble(date);
            writer.WriteShort(modelId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            mountId = reader.ReadInt();
            if (mountId < 0)
                throw new Exception("Forbidden value on mountId = " + mountId + ", it doesn't respect the following condition : mountId < 0");
            date = reader.ReadDouble();
            modelId = reader.ReadShort();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
        }
        
    }
    
}