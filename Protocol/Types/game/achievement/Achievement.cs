

// Generated on 04/17/2013 22:30:05
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class Achievement
    {
        public const short Id = 363;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short id;
        public Types.AchievementObjective[] finishedObjective;
        public Types.AchievementStartedObjective[] startedObjectives;
        
        public Achievement()
        {
        }
        
        public Achievement(short id, Types.AchievementObjective[] finishedObjective, Types.AchievementStartedObjective[] startedObjectives)
        {
            this.id = id;
            this.finishedObjective = finishedObjective;
            this.startedObjectives = startedObjectives;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(id);
            writer.WriteUShort((ushort)finishedObjective.Length);
            foreach (var entry in finishedObjective)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)startedObjectives.Length);
            foreach (var entry in startedObjectives)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            var limit = reader.ReadUShort();
            finishedObjective = new Types.AchievementObjective[limit];
            for (int i = 0; i < limit; i++)
            {
                 finishedObjective[i] = new Types.AchievementObjective();
                 finishedObjective[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            startedObjectives = new Types.AchievementStartedObjective[limit];
            for (int i = 0; i < limit; i++)
            {
                 startedObjectives[i] = new Types.AchievementStartedObjective();
                 startedObjectives[i].Deserialize(reader);
            }
        }
        
    }
    
}