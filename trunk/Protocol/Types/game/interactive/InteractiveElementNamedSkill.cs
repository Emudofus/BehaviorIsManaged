

// Generated on 09/23/2012 22:27:11
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class InteractiveElementNamedSkill : InteractiveElementSkill
    {
        public const short Id = 220;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int nameId;
        
        public InteractiveElementNamedSkill()
        {
        }
        
        public InteractiveElementNamedSkill(int skillId, int skillInstanceUid, int nameId)
         : base(skillId, skillInstanceUid)
        {
            this.nameId = nameId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(nameId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            nameId = reader.ReadInt();
            if (nameId < 0)
                throw new Exception("Forbidden value on nameId = " + nameId + ", it doesn't respect the following condition : nameId < 0");
        }
        
    }
    
}