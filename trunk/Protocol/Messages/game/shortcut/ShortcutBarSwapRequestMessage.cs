#region License GNU GPL
// ShortcutBarSwapRequestMessage.cs
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
    public class ShortcutBarSwapRequestMessage : NetworkMessage
    {
        public const uint Id = 6230;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte barType;
        public int firstSlot;
        public int secondSlot;
        
        public ShortcutBarSwapRequestMessage()
        {
        }
        
        public ShortcutBarSwapRequestMessage(sbyte barType, int firstSlot, int secondSlot)
        {
            this.barType = barType;
            this.firstSlot = firstSlot;
            this.secondSlot = secondSlot;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(barType);
            writer.WriteInt(firstSlot);
            writer.WriteInt(secondSlot);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            barType = reader.ReadSByte();
            if (barType < 0)
                throw new Exception("Forbidden value on barType = " + barType + ", it doesn't respect the following condition : barType < 0");
            firstSlot = reader.ReadInt();
            if (firstSlot < 0 || firstSlot > 99)
                throw new Exception("Forbidden value on firstSlot = " + firstSlot + ", it doesn't respect the following condition : firstSlot < 0 || firstSlot > 99");
            secondSlot = reader.ReadInt();
            if (secondSlot < 0 || secondSlot > 99)
                throw new Exception("Forbidden value on secondSlot = " + secondSlot + ", it doesn't respect the following condition : secondSlot < 0 || secondSlot > 99");
        }
        
    }
    
}