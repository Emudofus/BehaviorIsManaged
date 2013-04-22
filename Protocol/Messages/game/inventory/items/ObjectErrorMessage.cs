

// Generated on 04/17/2013 22:30:00
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ObjectErrorMessage : NetworkMessage
    {
        public const uint Id = 3004;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte reason;
        
        public ObjectErrorMessage()
        {
        }
        
        public ObjectErrorMessage(sbyte reason)
        {
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            reason = reader.ReadSByte();
        }
        
    }
    
}