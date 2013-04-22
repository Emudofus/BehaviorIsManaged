

// Generated on 04/17/2013 22:30:07
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameRolePlayMutantInformations : GameRolePlayHumanoidInformations
    {
        public const short Id = 3;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int monsterId;
        public sbyte powerLevel;
        
        public GameRolePlayMutantInformations()
        {
        }
        
        public GameRolePlayMutantInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name, Types.HumanInformations humanoidInfo, int accountId, int monsterId, sbyte powerLevel)
         : base(contextualId, look, disposition, name, humanoidInfo, accountId)
        {
            this.monsterId = monsterId;
            this.powerLevel = powerLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(monsterId);
            writer.WriteSByte(powerLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            monsterId = reader.ReadInt();
            powerLevel = reader.ReadSByte();
        }
        
    }
    
}