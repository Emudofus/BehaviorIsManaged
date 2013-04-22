

// Generated on 04/17/2013 22:29:57
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ExchangeMountPaddockAddMessage : NetworkMessage
    {
        public const uint Id = 6049;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.MountClientData mountDescription;
        
        public ExchangeMountPaddockAddMessage()
        {
        }
        
        public ExchangeMountPaddockAddMessage(Types.MountClientData mountDescription)
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