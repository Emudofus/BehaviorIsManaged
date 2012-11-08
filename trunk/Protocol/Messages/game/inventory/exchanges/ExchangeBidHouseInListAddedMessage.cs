#region License GNU GPL
// ExchangeBidHouseInListAddedMessage.cs
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
    public class ExchangeBidHouseInListAddedMessage : NetworkMessage
    {
        public const uint Id = 5949;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int itemUID;
        public int objGenericId;
        public short powerRate;
        public bool overMax;
        public Types.ObjectEffect[] effects;
        public int[] prices;
        
        public ExchangeBidHouseInListAddedMessage()
        {
        }
        
        public ExchangeBidHouseInListAddedMessage(int itemUID, int objGenericId, short powerRate, bool overMax, Types.ObjectEffect[] effects, int[] prices)
        {
            this.itemUID = itemUID;
            this.objGenericId = objGenericId;
            this.powerRate = powerRate;
            this.overMax = overMax;
            this.effects = effects;
            this.prices = prices;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(itemUID);
            writer.WriteInt(objGenericId);
            writer.WriteShort(powerRate);
            writer.WriteBoolean(overMax);
            writer.WriteUShort((ushort)effects.Length);
            foreach (var entry in effects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)prices.Length);
            foreach (var entry in prices)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            itemUID = reader.ReadInt();
            objGenericId = reader.ReadInt();
            powerRate = reader.ReadShort();
            overMax = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            effects = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 effects[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 effects[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            prices = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 prices[i] = reader.ReadInt();
            }
        }
        
    }
    
}