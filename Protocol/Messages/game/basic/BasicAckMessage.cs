

// Generated on 04/17/2013 22:29:38
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class BasicAckMessage : NetworkMessage
    {
        public const uint Id = 6362;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int seq;
        public short lastPacketId;
        
        public BasicAckMessage()
        {
        }
        
        public BasicAckMessage(int seq, short lastPacketId)
        {
            this.seq = seq;
            this.lastPacketId = lastPacketId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(seq);
            writer.WriteShort(lastPacketId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            seq = reader.ReadInt();
            if (seq < 0)
                throw new Exception("Forbidden value on seq = " + seq + ", it doesn't respect the following condition : seq < 0");
            lastPacketId = reader.ReadShort();
            if (lastPacketId < 0)
                throw new Exception("Forbidden value on lastPacketId = " + lastPacketId + ", it doesn't respect the following condition : lastPacketId < 0");
        }
        
    }
    
}