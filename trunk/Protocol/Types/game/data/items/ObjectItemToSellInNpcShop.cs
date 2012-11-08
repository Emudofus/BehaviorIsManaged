#region License GNU GPL
// ObjectItemToSellInNpcShop.cs
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
    public class ObjectItemToSellInNpcShop : ObjectItemMinimalInformation
    {
        public const short Id = 352;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int objectPrice;
        public string buyCriterion;
        
        public ObjectItemToSellInNpcShop()
        {
        }
        
        public ObjectItemToSellInNpcShop(short objectGID, short powerRate, bool overMax, Types.ObjectEffect[] effects, int objectPrice, string buyCriterion)
         : base(objectGID, powerRate, overMax, effects)
        {
            this.objectPrice = objectPrice;
            this.buyCriterion = buyCriterion;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(objectPrice);
            writer.WriteUTF(buyCriterion);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectPrice = reader.ReadInt();
            if (objectPrice < 0)
                throw new Exception("Forbidden value on objectPrice = " + objectPrice + ", it doesn't respect the following condition : objectPrice < 0");
            buyCriterion = reader.ReadUTF();
        }
        
    }
    
}