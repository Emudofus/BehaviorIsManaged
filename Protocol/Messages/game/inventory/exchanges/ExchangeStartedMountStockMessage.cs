

// Generated on 12/11/2012 19:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeStartedMountStockMessage : NetworkMessage
    {
        public const uint Id = 5984;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItem[] objectsInfos;
        
        public ExchangeStartedMountStockMessage()
        {
        }
        
        public ExchangeStartedMountStockMessage(Types.ObjectItem[] objectsInfos)
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
            objectsInfos = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos[i] = new Types.ObjectItem();
                 objectsInfos[i].Deserialize(reader);
            }
        }
        
    }
    
}