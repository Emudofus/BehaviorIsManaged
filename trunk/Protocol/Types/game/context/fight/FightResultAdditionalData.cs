

// Generated on 09/23/2012 22:27:08
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightResultAdditionalData
    {
        public const short Id = 191;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        
        public FightResultAdditionalData()
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