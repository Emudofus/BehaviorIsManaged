

// Generated on 04/17/2013 22:30:07
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameRolePlayGroupMonsterInformations : GameRolePlayActorInformations
    {
        public const short Id = 160;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.GroupMonsterStaticInformations staticInfos;
        public short ageBonus;
        public sbyte lootShare;
        public sbyte alignmentSide;
        public bool keyRingBonus;
        
        public GameRolePlayGroupMonsterInformations()
        {
        }
        
        public GameRolePlayGroupMonsterInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, Types.GroupMonsterStaticInformations staticInfos, short ageBonus, sbyte lootShare, sbyte alignmentSide, bool keyRingBonus)
         : base(contextualId, look, disposition)
        {
            this.staticInfos = staticInfos;
            this.ageBonus = ageBonus;
            this.lootShare = lootShare;
            this.alignmentSide = alignmentSide;
            this.keyRingBonus = keyRingBonus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(staticInfos.TypeId);
            staticInfos.Serialize(writer);
            writer.WriteShort(ageBonus);
            writer.WriteSByte(lootShare);
            writer.WriteSByte(alignmentSide);
            writer.WriteBoolean(keyRingBonus);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            staticInfos = Types.ProtocolTypeManager.GetInstance<Types.GroupMonsterStaticInformations>(reader.ReadShort());
            staticInfos.Deserialize(reader);
            ageBonus = reader.ReadShort();
            if (ageBonus < -1 || ageBonus > 1000)
                throw new Exception("Forbidden value on ageBonus = " + ageBonus + ", it doesn't respect the following condition : ageBonus < -1 || ageBonus > 1000");
            lootShare = reader.ReadSByte();
            if (lootShare < -1 || lootShare > 8)
                throw new Exception("Forbidden value on lootShare = " + lootShare + ", it doesn't respect the following condition : lootShare < -1 || lootShare > 8");
            alignmentSide = reader.ReadSByte();
            keyRingBonus = reader.ReadBoolean();
        }
        
    }
    
}