

// Generated on 12/11/2012 19:44:32
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class EntityMovementInformations
    {
        public const short Id = 63;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int id;
        public sbyte[] steps;
        
        public EntityMovementInformations()
        {
        }
        
        public EntityMovementInformations(int id, sbyte[] steps)
        {
            this.id = id;
            this.steps = steps;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteUShort((ushort)steps.Length);
            foreach (var entry in steps)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            var limit = reader.ReadUShort();
            steps = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 steps[i] = reader.ReadSByte();
            }
        }
        
    }
    
}