

// Generated on 12/11/2012 19:44:34
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameRolePlayNpcQuestFlag
    {
        public const short Id = 384;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short[] questsToValidId;
        public short[] questsToStartId;
        
        public GameRolePlayNpcQuestFlag()
        {
        }
        
        public GameRolePlayNpcQuestFlag(short[] questsToValidId, short[] questsToStartId)
        {
            this.questsToValidId = questsToValidId;
            this.questsToStartId = questsToStartId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)questsToValidId.Length);
            foreach (var entry in questsToValidId)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)questsToStartId.Length);
            foreach (var entry in questsToStartId)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            questsToValidId = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 questsToValidId[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            questsToStartId = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 questsToStartId[i] = reader.ReadShort();
            }
        }
        
    }
    
}