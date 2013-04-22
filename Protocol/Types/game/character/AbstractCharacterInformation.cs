

// Generated on 04/17/2013 22:30:05
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class AbstractCharacterInformation
    {
        public const short Id = 400;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int id;
        
        public AbstractCharacterInformation()
        {
        }
        
        public AbstractCharacterInformation(int id)
        {
            this.id = id;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
        }
        
    }
    
}