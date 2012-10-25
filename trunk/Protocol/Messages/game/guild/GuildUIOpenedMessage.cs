

// Generated on 10/25/2012 10:42:47
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GuildUIOpenedMessage : NetworkMessage
    {
        public const uint Id = 5561;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte type;
        
        public GuildUIOpenedMessage()
        {
        }
        
        public GuildUIOpenedMessage(sbyte type)
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