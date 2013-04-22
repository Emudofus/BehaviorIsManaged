

// Generated on 04/17/2013 22:29:55
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class TaxCollectorMovementRemoveMessage : NetworkMessage
    {
        public const uint Id = 5915;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int collectorId;
        
        public TaxCollectorMovementRemoveMessage()
        {
        }
        
        public TaxCollectorMovementRemoveMessage(int collectorId)
        {
            this.collectorId = collectorId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(collectorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            collectorId = reader.ReadInt();
        }
        
    }
    
}