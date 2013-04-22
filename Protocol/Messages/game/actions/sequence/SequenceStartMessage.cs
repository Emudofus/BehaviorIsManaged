

// Generated on 04/17/2013 22:29:37
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class SequenceStartMessage : NetworkMessage
    {
        public const uint Id = 955;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte sequenceType;
        public int authorId;
        
        public SequenceStartMessage()
        {
        }
        
        public SequenceStartMessage(sbyte sequenceType, int authorId)
        {
            this.sequenceType = sequenceType;
            this.authorId = authorId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(sequenceType);
            writer.WriteInt(authorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            sequenceType = reader.ReadSByte();
            authorId = reader.ReadInt();
        }
        
    }
    
}