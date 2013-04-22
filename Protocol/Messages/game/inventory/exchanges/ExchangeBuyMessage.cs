

// Generated on 04/17/2013 22:29:56
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeBuyMessage : NetworkMessage
    {
        public const uint Id = 5774;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectToBuyId;
        public int quantity;
        
        public ExchangeBuyMessage()
        {
        }
        
        public ExchangeBuyMessage(int objectToBuyId, int quantity)
        {
            this.objectToBuyId = objectToBuyId;
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(objectToBuyId);
            writer.WriteInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectToBuyId = reader.ReadInt();
            if (objectToBuyId < 0)
                throw new Exception("Forbidden value on objectToBuyId = " + objectToBuyId + ", it doesn't respect the following condition : objectToBuyId < 0");
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}