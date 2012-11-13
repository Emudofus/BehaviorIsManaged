

// Generated on 10/25/2012 10:42:48
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ObjectAveragePricesMessage : NetworkMessage
    {
        public const uint Id = 6335;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] ids;
        public int[] avgPrices;
        
        public ObjectAveragePricesMessage()
        {
        }
        
        public ObjectAveragePricesMessage(short[] ids, int[] avgPrices)
        {
            this.ids = ids;
            this.avgPrices = avgPrices;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)ids.Length);
            foreach (var entry in ids)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)avgPrices.Length);
            foreach (var entry in avgPrices)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            ids = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 ids[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            avgPrices = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 avgPrices[i] = reader.ReadInt();
            }
        }
        
    }
    
}