

// Generated on 12/11/2012 19:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class PrismWorldInformationRequestMessage : NetworkMessage
    {
        public const uint Id = 5985;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool join;
        
        public PrismWorldInformationRequestMessage()
        {
        }
        
        public PrismWorldInformationRequestMessage(bool join)
        {
            this.join = join;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(join);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            join = reader.ReadBoolean();
        }
        
    }
    
}