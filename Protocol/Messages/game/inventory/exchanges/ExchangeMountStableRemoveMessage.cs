

// Generated on 12/11/2012 19:44:26
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeMountStableRemoveMessage : NetworkMessage
    {
        public const uint Id = 5964;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double mountId;
        
        public ExchangeMountStableRemoveMessage()
        {
        }
        
        public ExchangeMountStableRemoveMessage(double mountId)
        {
            this.mountId = mountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(mountId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountId = reader.ReadDouble();
        }
        
    }
    
}