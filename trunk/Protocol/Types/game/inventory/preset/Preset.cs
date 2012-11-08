#region License GNU GPL
// Preset.cs
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
    public class Preset
    {
        public const short Id = 355;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public sbyte symbolId;
        public bool mount;
        public Types.PresetItem[] objects;
        
        public Preset()
        {
        }
        
        public Preset(sbyte presetId, sbyte symbolId, bool mount, Types.PresetItem[] objects)
        {
            this.presetId = presetId;
            this.symbolId = symbolId;
            this.mount = mount;
            this.objects = objects;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            writer.WriteSByte(symbolId);
            writer.WriteBoolean(mount);
            writer.WriteUShort((ushort)objects.Length);
            foreach (var entry in objects)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            symbolId = reader.ReadSByte();
            if (symbolId < 0)
                throw new Exception("Forbidden value on symbolId = " + symbolId + ", it doesn't respect the following condition : symbolId < 0");
            mount = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            objects = new Types.PresetItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objects[i] = new Types.PresetItem();
                 objects[i].Deserialize(reader);
            }
        }
        
    }
    
}