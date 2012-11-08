#region License GNU GPL
// ObjectUseOnCellMessage.cs
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
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ObjectUseOnCellMessage : ObjectUseMessage
    {
        public const uint Id = 3013;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short cells;
        
        public ObjectUseOnCellMessage()
        {
        }
        
        public ObjectUseOnCellMessage(int objectUID, short cells)
         : base(objectUID)
        {
            this.cells = cells;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(cells);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cells = reader.ReadShort();
            if (cells < 0 || cells > 559)
                throw new Exception("Forbidden value on cells = " + cells + ", it doesn't respect the following condition : cells < 0 || cells > 559");
        }
        
    }
    
}