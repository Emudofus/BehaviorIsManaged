

// Generated on 10/25/2012 10:42:56
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GroupMonsterStaticInformations
    {
        public const short Id = 140;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public Types.MonsterInGroupLightInformations mainCreatureLightInfos;
        public Types.MonsterInGroupInformations[] underlings;
        
        public GroupMonsterStaticInformations()
        {
        }
        
        public GroupMonsterStaticInformations(Types.MonsterInGroupLightInformations mainCreatureLightInfos, Types.MonsterInGroupInformations[] underlings)
        {
            this.mainCreatureLightInfos = mainCreatureLightInfos;
            this.underlings = underlings;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            mainCreatureLightInfos.Serialize(writer);
            writer.WriteUShort((ushort)underlings.Length);
            foreach (var entry in underlings)
            {
                 entry.Serialize(writer);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            mainCreatureLightInfos = new Types.MonsterInGroupLightInformations();
            mainCreatureLightInfos.Deserialize(reader);
            var limit = reader.ReadUShort();
            underlings = new Types.MonsterInGroupInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 underlings[i] = new Types.MonsterInGroupInformations();
                 underlings[i].Deserialize(reader);
            }
        }
        
    }
    
}