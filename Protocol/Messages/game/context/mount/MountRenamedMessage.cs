

// Generated on 04/17/2013 22:29:44
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MountRenamedMessage : NetworkMessage
    {
        public const uint Id = 5983;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double mountId;
        public string name;
        
        public MountRenamedMessage()
        {
        }
        
        public MountRenamedMessage(double mountId, string name)
        {
            this.mountId = mountId;
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(mountId);
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountId = reader.ReadDouble();
            name = reader.ReadUTF();
        }
        
    }
    
}