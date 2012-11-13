

// Generated on 10/25/2012 10:42:49
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeObjectUseInWorkshopMessage : NetworkMessage
    {
        public const uint Id = 6004;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectUID;
        public int quantity;
        
        public ExchangeObjectUseInWorkshopMessage()
        {
        }
        
        public ExchangeObjectUseInWorkshopMessage(int objectUID, int quantity)
        {
            this.objectUID = objectUID;
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(objectUID);
            writer.WriteInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            quantity = reader.ReadInt();
        }
        
    }
    
}