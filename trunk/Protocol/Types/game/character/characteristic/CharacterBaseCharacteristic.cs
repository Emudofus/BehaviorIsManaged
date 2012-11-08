#region License GNU GPL
// CharacterBaseCharacteristic.cs
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
    public class CharacterBaseCharacteristic
    {
        public const short Id = 4;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short @base;
        public short objectsAndMountBonus;
        public short alignGiftBonus;
        public short contextModif;
        
        public CharacterBaseCharacteristic()
        {
        }
        
        public CharacterBaseCharacteristic(short @base, short objectsAndMountBonus, short alignGiftBonus, short contextModif)
        {
            this.@base = @base;
            this.objectsAndMountBonus = objectsAndMountBonus;
            this.alignGiftBonus = alignGiftBonus;
            this.contextModif = contextModif;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(@base);
            writer.WriteShort(objectsAndMountBonus);
            writer.WriteShort(alignGiftBonus);
            writer.WriteShort(contextModif);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            @base = reader.ReadShort();
            objectsAndMountBonus = reader.ReadShort();
            alignGiftBonus = reader.ReadShort();
            contextModif = reader.ReadShort();
        }
        
    }
    
}