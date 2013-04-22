

// Generated on 04/17/2013 22:30:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ObjectModifiedMessage : NetworkMessage
    {
        public const uint Id = 3029;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItem @object;
        
        public ObjectModifiedMessage()
        {
        }
        
        public ObjectModifiedMessage(Types.ObjectItem @object)
        {
            this.@object = @object;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            @object.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            @object = new Types.ObjectItem();
            @object.Deserialize(reader);
        }
        
    }
    
}