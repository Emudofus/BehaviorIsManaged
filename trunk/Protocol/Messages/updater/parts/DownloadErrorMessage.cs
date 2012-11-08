#region License GNU GPL
// DownloadErrorMessage.cs
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
    public class DownloadErrorMessage : NetworkMessage
    {
        public const uint Id = 1513;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte errorId;
        public string message;
        public string helpUrl;
        
        public DownloadErrorMessage()
        {
        }
        
        public DownloadErrorMessage(sbyte errorId, string message, string helpUrl)
        {
            this.errorId = errorId;
            this.message = message;
            this.helpUrl = helpUrl;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(errorId);
            writer.WriteUTF(message);
            writer.WriteUTF(helpUrl);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            errorId = reader.ReadSByte();
            if (errorId < 0)
                throw new Exception("Forbidden value on errorId = " + errorId + ", it doesn't respect the following condition : errorId < 0");
            message = reader.ReadUTF();
            helpUrl = reader.ReadUTF();
        }
        
    }
    
}