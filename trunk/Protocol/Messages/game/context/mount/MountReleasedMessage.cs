

// Generated on 09/23/2012 22:26:50
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MountReleasedMessage : NetworkMessage
    {
        public const uint Id = 6308;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double mountId;
        
        public MountReleasedMessage()
        {
        }
        
        public MountReleasedMessage(double mountId)
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