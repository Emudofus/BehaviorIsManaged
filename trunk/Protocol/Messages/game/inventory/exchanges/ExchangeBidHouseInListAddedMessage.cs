

// Generated on 09/23/2012 22:26:59
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