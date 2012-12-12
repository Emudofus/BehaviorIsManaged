

// Generated on 12/11/2012 19:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class QuestValidatedMessage : NetworkMessage
    {
        public const uint Id = 6097;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public ushort questId;
        
        public QuestValidatedMessage()
        {
        }
        
        public QuestValidatedMessage(ushort questId)
        {
            this.questId = questId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort(questId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            questId = reader.ReadUShort();
            if (questId < 0 || questId > 65535)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0 || questId > 65535");
        }
        
    }
    
}