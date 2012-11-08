#region License GNU GPL
// ChatServerCopyMessage.cs
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
    public class ChatServerCopyMessage : ChatAbstractServerMessage
    {
        public const uint Id = 882;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int receiverId;
        public string receiverName;
        
        public ChatServerCopyMessage()
        {
        }
        
        public ChatServerCopyMessage(sbyte channel, string content, int timestamp, string fingerprint, int receiverId, string receiverName)
         : base(channel, content, timestamp, fingerprint)
        {
            this.receiverId = receiverId;
            this.receiverName = receiverName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(receiverId);
            writer.WriteUTF(receiverName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            receiverId = reader.ReadInt();
            if (receiverId < 0)
                throw new Exception("Forbidden value on receiverId = " + receiverId + ", it doesn't respect the following condition : receiverId < 0");
            receiverName = reader.ReadUTF();
        }
        
    }
    
}