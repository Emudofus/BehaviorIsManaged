

// Generated on 12/11/2012 19:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class BasicTimeMessage : NetworkMessage
    {
        public const uint Id = 175;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int timestamp;
        public short timezoneOffset;
        
        public BasicTimeMessage()
        {
        }
        
        public BasicTimeMessage(int timestamp, short timezoneOffset)
        {
            this.timestamp = timestamp;
            this.timezoneOffset = timezoneOffset;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(timestamp);
            writer.WriteShort(timezoneOffset);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            timestamp = reader.ReadInt();
            if (timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0");
            timezoneOffset = reader.ReadShort();
        }
        
    }
    
}