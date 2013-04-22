

// Generated on 04/17/2013 22:29:42
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class DisplayNumericalValueMessage : NetworkMessage
    {
        public const uint Id = 5808;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int entityId;
        public int value;
        public sbyte type;
        
        public DisplayNumericalValueMessage()
        {
        }
        
        public DisplayNumericalValueMessage(int entityId, int value, sbyte type)
        {
            this.entityId = entityId;
            this.value = value;
            this.type = type;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteInt(value);
            writer.WriteSByte(type);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            entityId = reader.ReadInt();
            value = reader.ReadInt();
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
    }
    
}