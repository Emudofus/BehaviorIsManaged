

// Generated on 04/17/2013 22:30:06
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class PlayerStatus
    {
        public const short Id = 415;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte statusId;
        
        public PlayerStatus()
        {
        }
        
        public PlayerStatus(sbyte statusId)
        {
            this.statusId = statusId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(statusId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            statusId = reader.ReadSByte();
            if (statusId < 0)
                throw new Exception("Forbidden value on statusId = " + statusId + ", it doesn't respect the following condition : statusId < 0");
        }
        
    }
    
}