#region License GNU GPL
// AlignmentSubAreasListMessage.cs
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
    public class AlignmentSubAreasListMessage : NetworkMessage
    {
        public const uint Id = 6059;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] angelsSubAreas;
        public short[] evilsSubAreas;
        
        public AlignmentSubAreasListMessage()
        {
        }
        
        public AlignmentSubAreasListMessage(short[] angelsSubAreas, short[] evilsSubAreas)
        {
            this.angelsSubAreas = angelsSubAreas;
            this.evilsSubAreas = evilsSubAreas;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)angelsSubAreas.Length);
            foreach (var entry in angelsSubAreas)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)evilsSubAreas.Length);
            foreach (var entry in evilsSubAreas)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            angelsSubAreas = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 angelsSubAreas[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            evilsSubAreas = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 evilsSubAreas[i] = reader.ReadShort();
            }
        }
        
    }
    
}