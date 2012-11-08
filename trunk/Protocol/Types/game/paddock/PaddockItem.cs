#region License GNU GPL
// PaddockItem.cs
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
    public class PaddockItem : ObjectItemInRolePlay
    {
        public const short Id = 185;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.ItemDurability durability;
        
        public PaddockItem()
        {
        }
        
        public PaddockItem(short cellId, short objectGID, Types.ItemDurability durability)
         : base(cellId, objectGID)
        {
            this.durability = durability;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            durability.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            durability = new Types.ItemDurability();
            durability.Deserialize(reader);
        }
        
    }
    
}