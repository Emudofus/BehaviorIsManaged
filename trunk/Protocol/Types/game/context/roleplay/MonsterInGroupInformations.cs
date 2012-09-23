

// Generated on 09/23/2012 22:27:09
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class MonsterInGroupInformations : MonsterInGroupLightInformations
    {
        public const short Id = 144;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.EntityLook look;
        
        public MonsterInGroupInformations()
        {
        }
        
        public MonsterInGroupInformations(int creatureGenericId, sbyte grade, Types.EntityLook look)
         : base(creatureGenericId, grade)
        {
            this.look = look;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            look.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            look = new Types.EntityLook();
            look.Deserialize(reader);
        }
        
    }
    
}