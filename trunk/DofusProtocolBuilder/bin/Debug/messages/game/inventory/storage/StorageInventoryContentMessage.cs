

// Generated on 10/25/2012 10:42:52
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class StorageInventoryContentMessage : InventoryContentMessage
    {
        public const uint Id = 5646;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public StorageInventoryContentMessage()
        {
        }
        
        public StorageInventoryContentMessage(Types.ObjectItem[] objects, int kamas)
         : base(objects, kamas)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}