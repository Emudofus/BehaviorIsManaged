

// Generated on 04/17/2013 22:29:44
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MountRenameRequestMessage : NetworkMessage
    {
        public const uint Id = 5987;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public double mountId;
        
        public MountRenameRequestMessage()
        {
        }
        
        public MountRenameRequestMessage(string name, double mountId)
        {
            this.name = name;
            this.mountId = mountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
            writer.WriteDouble(mountId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
            mountId = reader.ReadDouble();
        }
        
    }
    
}