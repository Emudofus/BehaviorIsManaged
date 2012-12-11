

// Generated on 12/11/2012 19:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class QuestStepStartedMessage : NetworkMessage
    {
        public const uint Id = 6096;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public ushort questId;
        public ushort stepId;
        
        public QuestStepStartedMessage()
        {
        }
        
        public QuestStepStartedMessage(ushort questId, ushort stepId)
        {
            this.questId = questId;
            this.stepId = stepId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort(questId);
            writer.WriteUShort(stepId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            questId = reader.ReadUShort();
            if (questId < 0 || questId > 65535)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0 || questId > 65535");
            stepId = reader.ReadUShort();
            if (stepId < 0 || stepId > 65535)
                throw new Exception("Forbidden value on stepId = " + stepId + ", it doesn't respect the following condition : stepId < 0 || stepId > 65535");
        }
        
    }
    
}