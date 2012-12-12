

// Generated on 12/11/2012 19:44:17
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MountDataMessage : NetworkMessage
    {
        public const uint Id = 5973;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.MountClientData mountData;
        
        public MountDataMessage()
        {
        }
        
        public MountDataMessage(Types.MountClientData mountData)
        {
            this.mountData = mountData;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            mountData.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountData = new Types.MountClientData();
            mountData.Deserialize(reader);
        }
        
    }
    
}