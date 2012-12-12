

// Generated on 12/11/2012 19:44:32
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class EntityDispositionInformations
    {
        public const short Id = 60;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short cellId;
        public sbyte direction;
        
        public EntityDispositionInformations()
        {
        }
        
        public EntityDispositionInformations(short cellId, sbyte direction)
        {
            this.cellId = cellId;
            this.direction = direction;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(cellId);
            writer.WriteSByte(direction);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            cellId = reader.ReadShort();
            if (cellId < -1 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < -1 || cellId > 559");
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
        }
        
    }
    
}