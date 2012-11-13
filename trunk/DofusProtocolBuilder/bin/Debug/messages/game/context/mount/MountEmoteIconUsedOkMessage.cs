

// Generated on 10/25/2012 10:42:39
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class MountEmoteIconUsedOkMessage : NetworkMessage
    {
        public const uint Id = 5978;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int mountId;
        public sbyte reactionType;
        
        public MountEmoteIconUsedOkMessage()
        {
        }
        
        public MountEmoteIconUsedOkMessage(int mountId, sbyte reactionType)
        {
            this.mountId = mountId;
            this.reactionType = reactionType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(mountId);
            writer.WriteSByte(reactionType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountId = reader.ReadInt();
            reactionType = reader.ReadSByte();
            if (reactionType < 0)
                throw new Exception("Forbidden value on reactionType = " + reactionType + ", it doesn't respect the following condition : reactionType < 0");
        }
        
    }
    
}