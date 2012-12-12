

// Generated on 12/11/2012 19:44:32
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightResultFighterListEntry : FightResultListEntry
    {
        public const short Id = 189;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int id;
        public bool alive;
        
        public FightResultFighterListEntry()
        {
        }
        
        public FightResultFighterListEntry(short outcome, Types.FightLoot rewards, int id, bool alive)
         : base(outcome, rewards)
        {
            this.id = id;
            this.alive = alive;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(id);
            writer.WriteBoolean(alive);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            id = reader.ReadInt();
            alive = reader.ReadBoolean();
        }
        
    }
    
}