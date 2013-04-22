

// Generated on 04/17/2013 22:29:51
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class QuestListMessage : NetworkMessage
    {
        public const uint Id = 5626;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short[] finishedQuestsIds;
        public short[] finishedQuestsCounts;
        public Types.QuestActiveInformations[] activeQuests;
        
        public QuestListMessage()
        {
        }
        
        public QuestListMessage(short[] finishedQuestsIds, short[] finishedQuestsCounts, Types.QuestActiveInformations[] activeQuests)
        {
            this.finishedQuestsIds = finishedQuestsIds;
            this.finishedQuestsCounts = finishedQuestsCounts;
            this.activeQuests = activeQuests;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)finishedQuestsIds.Length);
            foreach (var entry in finishedQuestsIds)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)finishedQuestsCounts.Length);
            foreach (var entry in finishedQuestsCounts)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)activeQuests.Length);
            foreach (var entry in activeQuests)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            finishedQuestsIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 finishedQuestsIds[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            finishedQuestsCounts = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 finishedQuestsCounts[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            activeQuests = new Types.QuestActiveInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 activeQuests[i] = Types.ProtocolTypeManager.GetInstance<Types.QuestActiveInformations>(reader.ReadShort());
                 activeQuests[i].Deserialize(reader);
            }
        }
        
    }
    
}