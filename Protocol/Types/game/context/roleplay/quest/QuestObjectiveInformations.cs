

// Generated on 04/17/2013 22:30:08
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class QuestObjectiveInformations
    {
        public const short Id = 385;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short objectiveId;
        public bool objectiveStatus;
        public string[] dialogParams;
        
        public QuestObjectiveInformations()
        {
        }
        
        public QuestObjectiveInformations(short objectiveId, bool objectiveStatus, string[] dialogParams)
        {
            this.objectiveId = objectiveId;
            this.objectiveStatus = objectiveStatus;
            this.dialogParams = dialogParams;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(objectiveId);
            writer.WriteBoolean(objectiveStatus);
            writer.WriteUShort((ushort)dialogParams.Length);
            foreach (var entry in dialogParams)
            {
                 writer.WriteUTF(entry);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            objectiveId = reader.ReadShort();
            if (objectiveId < 0)
                throw new Exception("Forbidden value on objectiveId = " + objectiveId + ", it doesn't respect the following condition : objectiveId < 0");
            objectiveStatus = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            dialogParams = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 dialogParams[i] = reader.ReadUTF();
            }
        }
        
    }
    
}