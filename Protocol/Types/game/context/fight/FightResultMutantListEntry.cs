

// Generated on 12/11/2012 19:44:32
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightResultMutantListEntry : FightResultFighterListEntry
    {
        public const short Id = 216;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public ushort level;
        
        public FightResultMutantListEntry()
        {
        }
        
        public FightResultMutantListEntry(short outcome, Types.FightLoot rewards, int id, bool alive, ushort level)
         : base(outcome, rewards, id, alive)
        {
            this.level = level;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort(level);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            level = reader.ReadUShort();
            if (level < 0 || level > 65535)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0 || level > 65535");
        }
        
    }
    
}