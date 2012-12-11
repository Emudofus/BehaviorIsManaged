

// Generated on 12/11/2012 19:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeObjectModifiedMessage : ExchangeObjectMessage
    {
        public const uint Id = 5519;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItem @object;
        
        public ExchangeObjectModifiedMessage()
        {
        }
        
        public ExchangeObjectModifiedMessage(bool remote, Types.ObjectItem @object)
         : base(remote)
        {
            this.@object = @object;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            @object.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            @object = new Types.ObjectItem();
            @object.Deserialize(reader);
        }
        
    }
    
}