

// Generated on 12/11/2012 19:44:26
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeMountStableAddMessage : NetworkMessage
    {
        public const uint Id = 5971;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.MountClientData mountDescription;
        
        public ExchangeMountStableAddMessage()
        {
        }
        
        public ExchangeMountStableAddMessage(Types.MountClientData mountDescription)
        {
            this.mountDescription = mountDescription;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            mountDescription.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountDescription = new Types.MountClientData();
            mountDescription.Deserialize(reader);
        }
        
    }
    
}