#region License GNU GPL
// StatedElement.cs
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
    public class StatedElement
    {
        public const short Id = 108;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int elementId;
        public short elementCellId;
        public int elementState;
        
        public StatedElement()
        {
        }
        
        public StatedElement(int elementId, short elementCellId, int elementState)
        {
            this.elementId = elementId;
            this.elementCellId = elementCellId;
            this.elementState = elementState;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(elementId);
            writer.WriteShort(elementCellId);
            writer.WriteInt(elementState);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            elementId = reader.ReadInt();
            if (elementId < 0)
                throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
            elementCellId = reader.ReadShort();
            if (elementCellId < 0 || elementCellId > 559)
                throw new Exception("Forbidden value on elementCellId = " + elementCellId + ", it doesn't respect the following condition : elementCellId < 0 || elementCellId > 559");
            elementState = reader.ReadInt();
            if (elementState < 0)
                throw new Exception("Forbidden value on elementState = " + elementState + ", it doesn't respect the following condition : elementState < 0");
        }
        
    }
    
}