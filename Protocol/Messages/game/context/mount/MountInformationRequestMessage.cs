

// Generated on 04/17/2013 22:29:43
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MountInformationRequestMessage : NetworkMessage
    {
        public const uint Id = 5972;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double id;
        public double time;
        
        public MountInformationRequestMessage()
        {
        }
        
        public MountInformationRequestMessage(double id, double time)
        {
            this.id = id;
            this.time = time;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(id);
            writer.WriteDouble(time);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadDouble();
            time = reader.ReadDouble();
        }
        
    }
    
}