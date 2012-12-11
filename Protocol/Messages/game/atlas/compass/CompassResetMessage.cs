

// Generated on 12/11/2012 19:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class CompassResetMessage : NetworkMessage
    {
        public const uint Id = 5584;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte type;
        
        public CompassResetMessage()
        {
        }
        
        public CompassResetMessage(sbyte type)
        {
            this.type = type;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(type);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
    }
    
}