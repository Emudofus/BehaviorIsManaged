

// Generated on 09/23/2012 22:26:43
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class IdentificationFailedMessage : NetworkMessage
    {
        public const uint Id = 20;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte reason;
        
        public IdentificationFailedMessage()
        {
        }
        
        public IdentificationFailedMessage(sbyte reason)
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
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
        }
        
    }
    
}