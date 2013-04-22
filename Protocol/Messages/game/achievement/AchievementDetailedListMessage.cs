

// Generated on 04/17/2013 22:29:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class AchievementDetailedListMessage : NetworkMessage
    {
        public const uint Id = 6358;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.Achievement[] startedAchievements;
        public Types.Achievement[] finishedAchievements;
        
        public AchievementDetailedListMessage()
        {
        }
        
        public AchievementDetailedListMessage(Types.Achievement[] startedAchievements, Types.Achievement[] finishedAchievements)
        {
            this.startedAchievements = startedAchievements;
            this.finishedAchievements = finishedAchievements;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)startedAchievements.Length);
            foreach (var entry in startedAchievements)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)finishedAchievements.Length);
            foreach (var entry in finishedAchievements)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            startedAchievements = new Types.Achievement[limit];
            for (int i = 0; i < limit; i++)
            {
                 startedAchievements[i] = new Types.Achievement();
                 startedAchievements[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            finishedAchievements = new Types.Achievement[limit];
            for (int i = 0; i < limit; i++)
            {
                 finishedAchievements[i] = new Types.Achievement();
                 finishedAchievements[i].Deserialize(reader);
            }
        }
        
    }
    
}