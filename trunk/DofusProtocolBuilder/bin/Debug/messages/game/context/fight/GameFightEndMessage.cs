

// Generated on 10/25/2012 10:42:38
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightEndMessage : NetworkMessage
    {
        public const uint Id = 720;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int duration;
        public short ageBonus;
        public short lootShareLimitMalus;
        public Types.FightResultListEntry[] results;
        
        public GameFightEndMessage()
        {
        }
        
        public GameFightEndMessage(int duration, short ageBonus, short lootShareLimitMalus, Types.FightResultListEntry[] results)
        {
            this.duration = duration;
            this.ageBonus = ageBonus;
            this.lootShareLimitMalus = lootShareLimitMalus;
            this.results = results;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(duration);
            writer.WriteShort(ageBonus);
            writer.WriteShort(lootShareLimitMalus);
            writer.WriteUShort((ushort)results.Length);
            foreach (var entry in results)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            duration = reader.ReadInt();
            if (duration < 0)
                throw new Exception("Forbidden value on duration = " + duration + ", it doesn't respect the following condition : duration < 0");
            ageBonus = reader.ReadShort();
            lootShareLimitMalus = reader.ReadShort();
            var limit = reader.ReadUShort();
            results = new Types.FightResultListEntry[limit];
            for (int i = 0; i < limit; i++)
            {
                 results[i] = Types.ProtocolTypeManager.GetInstance<Types.FightResultListEntry>(reader.ReadShort());
                 results[i].Deserialize(reader);
            }
        }
        
    }
    
}