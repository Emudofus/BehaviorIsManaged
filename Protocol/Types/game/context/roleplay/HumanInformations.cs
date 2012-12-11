

// Generated on 12/11/2012 19:44:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class HumanInformations
    {
        public const short Id = 157;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public Types.ActorRestrictionsInformations restrictions;
        public bool sex;
        public Types.HumanOption[] options;
        
        public HumanInformations()
        {
        }
        
        public HumanInformations(Types.ActorRestrictionsInformations restrictions, bool sex, Types.HumanOption[] options)
        {
            this.restrictions = restrictions;
            this.sex = sex;
            this.options = options;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            restrictions.Serialize(writer);
            writer.WriteBoolean(sex);
            writer.WriteUShort((ushort)options.Length);
            foreach (var entry in options)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            restrictions = new Types.ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
            sex = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            options = new Types.HumanOption[limit];
            for (int i = 0; i < limit; i++)
            {
                 options[i] = Types.ProtocolTypeManager.GetInstance<Types.HumanOption>(reader.ReadShort());
                 options[i].Deserialize(reader);
            }
        }
        
    }
    
}