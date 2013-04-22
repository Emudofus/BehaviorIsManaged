

// Generated on 04/17/2013 22:30:08
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class HumanOption
    {
        public const short Id = 406;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        
        public HumanOption()
        {
        }
        
        
        public virtual void Serialize(IDataWriter writer)
        {
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}