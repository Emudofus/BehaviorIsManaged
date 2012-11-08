#region License GNU GPL
// BasicLatencyStatsMessage.cs
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
    public class BasicLatencyStatsMessage : NetworkMessage
    {
        public const uint Id = 5663;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public ushort latency;
        public short sampleCount;
        public short max;
        
        public BasicLatencyStatsMessage()
        {
        }
        
        public BasicLatencyStatsMessage(ushort latency, short sampleCount, short max)
        {
            this.latency = latency;
            this.sampleCount = sampleCount;
            this.max = max;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort(latency);
            writer.WriteShort(sampleCount);
            writer.WriteShort(max);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            latency = reader.ReadUShort();
            if (latency < 0 || latency > 65535)
                throw new Exception("Forbidden value on latency = " + latency + ", it doesn't respect the following condition : latency < 0 || latency > 65535");
            sampleCount = reader.ReadShort();
            if (sampleCount < 0)
                throw new Exception("Forbidden value on sampleCount = " + sampleCount + ", it doesn't respect the following condition : sampleCount < 0");
            max = reader.ReadShort();
            if (max < 0)
                throw new Exception("Forbidden value on max = " + max + ", it doesn't respect the following condition : max < 0");
        }
        
    }
    
}