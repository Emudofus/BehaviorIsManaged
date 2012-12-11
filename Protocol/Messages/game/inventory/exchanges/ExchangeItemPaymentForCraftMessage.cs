

// Generated on 12/11/2012 19:44:26
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeItemPaymentForCraftMessage : NetworkMessage
    {
        public const uint Id = 5831;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool onlySuccess;
        public Types.ObjectItemNotInContainer @object;
        
        public ExchangeItemPaymentForCraftMessage()
        {
        }
        
        public ExchangeItemPaymentForCraftMessage(bool onlySuccess, Types.ObjectItemNotInContainer @object)
        {
            this.onlySuccess = onlySuccess;
            this.@object = @object;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(onlySuccess);
            @object.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            onlySuccess = reader.ReadBoolean();
            @object = new Types.ObjectItemNotInContainer();
            @object.Deserialize(reader);
        }
        
    }
    
}