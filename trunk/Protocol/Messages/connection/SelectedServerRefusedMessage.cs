#region License GNU GPL
// SelectedServerRefusedMessage.cs
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
    public class SelectedServerRefusedMessage : NetworkMessage
    {
        public const uint Id = 41;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short serverId;
        public sbyte error;
        public sbyte serverStatus;
        
        public SelectedServerRefusedMessage()
        {
        }
        
        public SelectedServerRefusedMessage(short serverId, sbyte error, sbyte serverStatus)
        {
            this.serverId = serverId;
            this.error = error;
            this.serverStatus = serverStatus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(serverId);
            writer.WriteSByte(error);
            writer.WriteSByte(serverStatus);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            serverId = reader.ReadShort();
            error = reader.ReadSByte();
            if (error < 0)
                throw new Exception("Forbidden value on error = " + error + ", it doesn't respect the following condition : error < 0");
            serverStatus = reader.ReadSByte();
            if (serverStatus < 0)
                throw new Exception("Forbidden value on serverStatus = " + serverStatus + ", it doesn't respect the following condition : serverStatus < 0");
        }
        
    }
    
}