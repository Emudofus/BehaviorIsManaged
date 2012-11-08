#region License GNU GPL
// ExchangeStartedWithPodsMessage.cs
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
    public class ExchangeStartedWithPodsMessage : ExchangeStartedMessage
    {
        public const uint Id = 6129;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int firstCharacterId;
        public int firstCharacterCurrentWeight;
        public int firstCharacterMaxWeight;
        public int secondCharacterId;
        public int secondCharacterCurrentWeight;
        public int secondCharacterMaxWeight;
        
        public ExchangeStartedWithPodsMessage()
        {
        }
        
        public ExchangeStartedWithPodsMessage(sbyte exchangeType, int firstCharacterId, int firstCharacterCurrentWeight, int firstCharacterMaxWeight, int secondCharacterId, int secondCharacterCurrentWeight, int secondCharacterMaxWeight)
         : base(exchangeType)
        {
            this.firstCharacterId = firstCharacterId;
            this.firstCharacterCurrentWeight = firstCharacterCurrentWeight;
            this.firstCharacterMaxWeight = firstCharacterMaxWeight;
            this.secondCharacterId = secondCharacterId;
            this.secondCharacterCurrentWeight = secondCharacterCurrentWeight;
            this.secondCharacterMaxWeight = secondCharacterMaxWeight;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(firstCharacterId);
            writer.WriteInt(firstCharacterCurrentWeight);
            writer.WriteInt(firstCharacterMaxWeight);
            writer.WriteInt(secondCharacterId);
            writer.WriteInt(secondCharacterCurrentWeight);
            writer.WriteInt(secondCharacterMaxWeight);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            firstCharacterId = reader.ReadInt();
            firstCharacterCurrentWeight = reader.ReadInt();
            if (firstCharacterCurrentWeight < 0)
                throw new Exception("Forbidden value on firstCharacterCurrentWeight = " + firstCharacterCurrentWeight + ", it doesn't respect the following condition : firstCharacterCurrentWeight < 0");
            firstCharacterMaxWeight = reader.ReadInt();
            if (firstCharacterMaxWeight < 0)
                throw new Exception("Forbidden value on firstCharacterMaxWeight = " + firstCharacterMaxWeight + ", it doesn't respect the following condition : firstCharacterMaxWeight < 0");
            secondCharacterId = reader.ReadInt();
            secondCharacterCurrentWeight = reader.ReadInt();
            if (secondCharacterCurrentWeight < 0)
                throw new Exception("Forbidden value on secondCharacterCurrentWeight = " + secondCharacterCurrentWeight + ", it doesn't respect the following condition : secondCharacterCurrentWeight < 0");
            secondCharacterMaxWeight = reader.ReadInt();
            if (secondCharacterMaxWeight < 0)
                throw new Exception("Forbidden value on secondCharacterMaxWeight = " + secondCharacterMaxWeight + ", it doesn't respect the following condition : secondCharacterMaxWeight < 0");
        }
        
    }
    
}