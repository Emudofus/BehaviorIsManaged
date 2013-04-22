

// Generated on 04/17/2013 22:29:59
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeStartOkTaxCollectorMessage : NetworkMessage
    {
        public const uint Id = 5780;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int collectorId;
        public Types.ObjectItem[] objectsInfos;
        public int goldInfo;
        
        public ExchangeStartOkTaxCollectorMessage()
        {
        }
        
        public ExchangeStartOkTaxCollectorMessage(int collectorId, Types.ObjectItem[] objectsInfos, int goldInfo)
        {
            this.collectorId = collectorId;
            this.objectsInfos = objectsInfos;
            this.goldInfo = goldInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(collectorId);
            writer.WriteUShort((ushort)objectsInfos.Length);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
            }
            writer.WriteInt(goldInfo);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            collectorId = reader.ReadInt();
            var limit = reader.ReadUShort();
            objectsInfos = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos[i] = new Types.ObjectItem();
                 objectsInfos[i].Deserialize(reader);
            }
            goldInfo = reader.ReadInt();
            if (goldInfo < 0)
                throw new Exception("Forbidden value on goldInfo = " + goldInfo + ", it doesn't respect the following condition : goldInfo < 0");
        }
        
    }
    
}