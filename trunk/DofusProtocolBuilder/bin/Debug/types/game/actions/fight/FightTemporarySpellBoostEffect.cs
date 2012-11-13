

// Generated on 10/25/2012 10:42:55
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightTemporarySpellBoostEffect : FightTemporaryBoostEffect
    {
        public const short Id = 207;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short boostedSpellId;
        
        public FightTemporarySpellBoostEffect()
        {
        }
        
        public FightTemporarySpellBoostEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, int parentBoostUid, short delta, short boostedSpellId)
         : base(uid, targetId, turnDuration, dispelable, spellId, parentBoostUid, delta)
        {
            this.boostedSpellId = boostedSpellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(boostedSpellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            boostedSpellId = reader.ReadShort();
            if (boostedSpellId < 0)
                throw new Exception("Forbidden value on boostedSpellId = " + boostedSpellId + ", it doesn't respect the following condition : boostedSpellId < 0");
        }
        
    }
    
}