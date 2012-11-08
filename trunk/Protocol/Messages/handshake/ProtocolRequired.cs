#region License GNU GPL
// ProtocolRequired.cs
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
    public class ProtocolRequired : NetworkMessage
    {
        public const uint Id = 1;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int requiredVersion;
        public int currentVersion;
        
        public ProtocolRequired()
        {
        }
        
        public ProtocolRequired(int requiredVersion, int currentVersion)
        {
            this.requiredVersion = requiredVersion;
            this.currentVersion = currentVersion;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(requiredVersion);
            writer.WriteInt(currentVersion);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            requiredVersion = reader.ReadInt();
            if (requiredVersion < 0)
                throw new Exception("Forbidden value on requiredVersion = " + requiredVersion + ", it doesn't respect the following condition : requiredVersion < 0");
            currentVersion = reader.ReadInt();
            if (currentVersion < 0)
                throw new Exception("Forbidden value on currentVersion = " + currentVersion + ", it doesn't respect the following condition : currentVersion < 0");
        }
        
    }
    
}