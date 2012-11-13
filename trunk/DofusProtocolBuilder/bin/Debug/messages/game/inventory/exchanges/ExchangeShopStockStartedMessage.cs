

// Generated on 10/25/2012 10:42:50
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeShopStockStartedMessage : NetworkMessage
    {
        public const uint Id = 5910;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItemToSell[] objectsInfos;
        
        public ExchangeShopStockStartedMessage()
        {
        }
        
        public ExchangeShopStockStartedMessage(Types.ObjectItemToSell[] objectsInfos)
        {
            this.objectsInfos = objectsInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)objectsInfos.Length);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            objectsInfos = new Types.ObjectItemToSell[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos[i] = new Types.ObjectItemToSell();
                 objectsInfos[i].Deserialize(reader);
            }
        }
        
    }
    
}