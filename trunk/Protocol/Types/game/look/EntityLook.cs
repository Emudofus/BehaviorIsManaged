#region License GNU GPL
// EntityLook.cs
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
    public class EntityLook
    {
        public const short Id = 55;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short bonesId;
        public short[] skins;
        public int[] indexedColors;
        public short[] scales;
        public Types.SubEntity[] subentities;
        
        public EntityLook()
        {
        }
        
        public EntityLook(short bonesId, short[] skins, int[] indexedColors, short[] scales, Types.SubEntity[] subentities)
        {
            this.bonesId = bonesId;
            this.skins = skins;
            this.indexedColors = indexedColors;
            this.scales = scales;
            this.subentities = subentities;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(bonesId);
            writer.WriteUShort((ushort)skins.Length);
            foreach (var entry in skins)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)indexedColors.Length);
            foreach (var entry in indexedColors)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)scales.Length);
            foreach (var entry in scales)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)subentities.Length);
            foreach (var entry in subentities)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            bonesId = reader.ReadShort();
            if (bonesId < 0)
                throw new Exception("Forbidden value on bonesId = " + bonesId + ", it doesn't respect the following condition : bonesId < 0");
            var limit = reader.ReadUShort();
            skins = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 skins[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            indexedColors = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 indexedColors[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            scales = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 scales[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            subentities = new Types.SubEntity[limit];
            for (int i = 0; i < limit; i++)
            {
                 subentities[i] = new Types.SubEntity();
                 subentities[i].Deserialize(reader);
            }
        }
        
    }
    
}