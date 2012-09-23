

// Generated on 09/23/2012 22:27:10
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class ObjectEffectCreature : ObjectEffect
    {
        public const short Id = 71;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short monsterFamilyId;
        
        public ObjectEffectCreature()
        {
        }
        
        public ObjectEffectCreature(short actionId, short monsterFamilyId)
         : base(actionId)
        {
            this.monsterFamilyId = monsterFamilyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(monsterFamilyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            monsterFamilyId = reader.ReadShort();
            if (monsterFamilyId < 0)
                throw new Exception("Forbidden value on monsterFamilyId = " + monsterFamilyId + ", it doesn't respect the following condition : monsterFamilyId < 0");
        }
        
    }
    
}