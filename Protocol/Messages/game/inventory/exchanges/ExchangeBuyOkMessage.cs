

// Generated on 04/17/2013 22:29:56
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeBuyOkMessage : NetworkMessage
    {
        public const uint Id = 5759;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeBuyOkMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}