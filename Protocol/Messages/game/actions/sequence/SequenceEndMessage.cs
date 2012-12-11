

// Generated on 12/11/2012 19:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class SequenceEndMessage : NetworkMessage
    {
        public const uint Id = 956;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short actionId;
        public int authorId;
        public sbyte sequenceType;
        
        public SequenceEndMessage()
        {
        }
        
        public SequenceEndMessage(short actionId, int authorId, sbyte sequenceType)
        {
            this.actionId = actionId;
            this.authorId = authorId;
            this.sequenceType = sequenceType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(actionId);
            writer.WriteInt(authorId);
            writer.WriteSByte(sequenceType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            actionId = reader.ReadShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            authorId = reader.ReadInt();
            sequenceType = reader.ReadSByte();
        }
        
    }
    
}