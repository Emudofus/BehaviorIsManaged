

// Generated on 04/17/2013 22:30:03
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class OrnamentSelectRequestMessage : NetworkMessage
    {
        public const uint Id = 6374;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short ornamentId;
        
        public OrnamentSelectRequestMessage()
        {
        }
        
        public OrnamentSelectRequestMessage(short ornamentId)
        {
            this.ornamentId = ornamentId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(ornamentId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            ornamentId = reader.ReadShort();
            if (ornamentId < 0)
                throw new Exception("Forbidden value on ornamentId = " + ornamentId + ", it doesn't respect the following condition : ornamentId < 0");
        }
        
    }
    
}