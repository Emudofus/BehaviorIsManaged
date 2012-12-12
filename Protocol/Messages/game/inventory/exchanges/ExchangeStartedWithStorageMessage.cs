

// Generated on 12/11/2012 19:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeStartedWithStorageMessage : ExchangeStartedMessage
    {
        public const uint Id = 6236;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int storageMaxSlot;
        
        public ExchangeStartedWithStorageMessage()
        {
        }
        
        public ExchangeStartedWithStorageMessage(sbyte exchangeType, int storageMaxSlot)
         : base(exchangeType)
        {
            this.storageMaxSlot = storageMaxSlot;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(storageMaxSlot);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            storageMaxSlot = reader.ReadInt();
            if (storageMaxSlot < 0)
                throw new Exception("Forbidden value on storageMaxSlot = " + storageMaxSlot + ", it doesn't respect the following condition : storageMaxSlot < 0");
        }
        
    }
    
}