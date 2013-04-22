

// Generated on 04/17/2013 22:29:58
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeRemovedPaymentForCraftMessage : NetworkMessage
    {
        public const uint Id = 6031;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool onlySuccess;
        public int objectUID;
        
        public ExchangeRemovedPaymentForCraftMessage()
        {
        }
        
        public ExchangeRemovedPaymentForCraftMessage(bool onlySuccess, int objectUID)
        {
            this.onlySuccess = onlySuccess;
            this.objectUID = objectUID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(onlySuccess);
            writer.WriteInt(objectUID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            onlySuccess = reader.ReadBoolean();
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
        }
        
    }
    
}