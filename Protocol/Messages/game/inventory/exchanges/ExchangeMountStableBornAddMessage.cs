

// Generated on 12/11/2012 19:44:26
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeMountStableBornAddMessage : ExchangeMountStableAddMessage
    {
        public const uint Id = 5966;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeMountStableBornAddMessage()
        {
        }
        
        public ExchangeMountStableBornAddMessage(Types.MountClientData mountDescription)
         : base(mountDescription)
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