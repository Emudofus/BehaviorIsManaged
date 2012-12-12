

// Generated on 12/11/2012 19:44:32
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class FightTemporaryBoostWeaponDamagesEffect : FightTemporaryBoostEffect
    {
        public const short Id = 211;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short weaponTypeId;
        
        public FightTemporaryBoostWeaponDamagesEffect()
        {
        }
        
        public FightTemporaryBoostWeaponDamagesEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, int parentBoostUid, short delta, short weaponTypeId)
         : base(uid, targetId, turnDuration, dispelable, spellId, parentBoostUid, delta)
        {
            this.weaponTypeId = weaponTypeId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(weaponTypeId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            weaponTypeId = reader.ReadShort();
        }
        
    }
    
}