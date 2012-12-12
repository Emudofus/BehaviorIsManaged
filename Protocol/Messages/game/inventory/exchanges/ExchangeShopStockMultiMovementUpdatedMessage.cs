

// Generated on 12/11/2012 19:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeShopStockMultiMovementUpdatedMessage : NetworkMessage
    {
        public const uint Id = 6038;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItemToSell[] objectInfoList;
        
        public ExchangeShopStockMultiMovementUpdatedMessage()
        {
        }
        
        public ExchangeShopStockMultiMovementUpdatedMessage(Types.ObjectItemToSell[] objectInfoList)
        {
            this.objectInfoList = objectInfoList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)objectInfoList.Length);
            foreach (var entry in objectInfoList)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            objectInfoList = new Types.ObjectItemToSell[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectInfoList[i] = new Types.ObjectItemToSell();
                 objectInfoList[i].Deserialize(reader);
            }
        }
        
    }
    
}