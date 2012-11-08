#region License GNU GPL
// StartupActionFinishedMessage.cs
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
    public class StartupActionFinishedMessage : NetworkMessage
    {
        public const uint Id = 1304;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool success;
        public bool automaticAction;
        public int actionId;
        
        public StartupActionFinishedMessage()
        {
        }
        
        public StartupActionFinishedMessage(bool success, bool automaticAction, int actionId)
        {
            this.success = success;
            this.automaticAction = automaticAction;
            this.actionId = actionId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, success);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, automaticAction);
            writer.WriteByte(flag1);
            writer.WriteInt(actionId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            success = BooleanByteWrapper.GetFlag(flag1, 0);
            automaticAction = BooleanByteWrapper.GetFlag(flag1, 1);
            actionId = reader.ReadInt();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
        }
        
    }
    
}