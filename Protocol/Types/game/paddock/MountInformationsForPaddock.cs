

// Generated on 04/17/2013 22:30:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class MountInformationsForPaddock
    {
        public const short Id = 184;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int modelId;
        public string name;
        public string ownerName;
        
        public MountInformationsForPaddock()
        {
        }
        
        public MountInformationsForPaddock(int modelId, string name, string ownerName)
        {
            this.modelId = modelId;
            this.name = name;
            this.ownerName = ownerName;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(modelId);
            writer.WriteUTF(name);
            writer.WriteUTF(ownerName);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            modelId = reader.ReadInt();
            name = reader.ReadUTF();
            ownerName = reader.ReadUTF();
        }
        
    }
    
}