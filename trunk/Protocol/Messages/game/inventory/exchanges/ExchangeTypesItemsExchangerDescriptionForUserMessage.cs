

// Generated on 09/23/2012 22:27:02
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeTypesItemsExchangerDescriptionForUserMessage : NetworkMessage
    {
        public const uint Id = 5752;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.BidExchangerObjectInfo[] itemTypeDescriptions;
        
        public ExchangeTypesItemsExchangerDescriptionForUserMessage()
        {
        }
        
        public ExchangeTypesItemsExchangerDescriptionForUserMessage(Types.BidExchangerObjectInfo[] itemTypeDescriptions)
        {
            this.itemTypeDescriptions = itemTypeDescriptions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)itemTypeDescriptions.Length);
            foreach (var entry in itemTypeDescriptions)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            itemTypeDescriptions = new Types.BidExchangerObjectInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 itemTypeDescriptions[i] = new Types.BidExchangerObjectInfo();
                 itemTypeDescriptions[i].Deserialize(reader);
            }
        }
        
    }
    
}