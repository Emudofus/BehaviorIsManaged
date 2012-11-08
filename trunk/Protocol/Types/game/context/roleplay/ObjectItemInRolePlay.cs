#region License GNU GPL
// ObjectItemInRolePlay.cs
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
    public class ObjectItemInRolePlay
    {
        public const short Id = 198;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short cellId;
        public short objectGID;
        
        public ObjectItemInRolePlay()
        {
        }
        
        public ObjectItemInRolePlay(short cellId, short objectGID)
        {
            this.cellId = cellId;
            this.objectGID = objectGID;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(cellId);
            writer.WriteShort(objectGID);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            cellId = reader.ReadShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
            objectGID = reader.ReadShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
        }
        
    }
    
}