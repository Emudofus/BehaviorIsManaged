#region License GNU GPL
// GameServerInformations.cs
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
    public class GameServerInformations
    {
        public const short Id = 25;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public ushort id;
        public sbyte status;
        public sbyte completion;
        public bool isSelectable;
        public sbyte charactersCount;
        public double date;
        
        public GameServerInformations()
        {
        }
        
        public GameServerInformations(ushort id, sbyte status, sbyte completion, bool isSelectable, sbyte charactersCount, double date)
        {
            this.id = id;
            this.status = status;
            this.completion = completion;
            this.isSelectable = isSelectable;
            this.charactersCount = charactersCount;
            this.date = date;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUShort(id);
            writer.WriteSByte(status);
            writer.WriteSByte(completion);
            writer.WriteBoolean(isSelectable);
            writer.WriteSByte(charactersCount);
            writer.WriteDouble(date);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadUShort();
            if (id < 0 || id > 65535)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0 || id > 65535");
            status = reader.ReadSByte();
            if (status < 0)
                throw new Exception("Forbidden value on status = " + status + ", it doesn't respect the following condition : status < 0");
            completion = reader.ReadSByte();
            if (completion < 0)
                throw new Exception("Forbidden value on completion = " + completion + ", it doesn't respect the following condition : completion < 0");
            isSelectable = reader.ReadBoolean();
            charactersCount = reader.ReadSByte();
            if (charactersCount < 0)
                throw new Exception("Forbidden value on charactersCount = " + charactersCount + ", it doesn't respect the following condition : charactersCount < 0");
            date = reader.ReadDouble();
        }
        
    }
    
}