

// Generated on 12/11/2012 19:44:26
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeObjectMoveKamaMessage : NetworkMessage
    {
        public const uint Id = 5520;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int quantity;
        
        public ExchangeObjectMoveKamaMessage()
        {
        }
        
        public ExchangeObjectMoveKamaMessage(int quantity)
        {
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            quantity = reader.ReadInt();
        }
        
    }
    
}