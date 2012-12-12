

// Generated on 12/11/2012 19:44:32
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightTemporaryBoostEffect : AbstractFightDispellableEffect
    {
        public const short Id = 209;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short delta;
        
        public FightTemporaryBoostEffect()
        {
        }
        
        public FightTemporaryBoostEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, int parentBoostUid, short delta)
         : base(uid, targetId, turnDuration, dispelable, spellId, parentBoostUid)
        {
            this.delta = delta;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(delta);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            delta = reader.ReadShort();
        }
        
    }
    
}