

// Generated on 09/23/2012 22:27:09
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GroupMonsterStaticInformationsWithAlternatives : GroupMonsterStaticInformations
    {
        public const short Id = 396;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.AlternativeMonstersInGroupLightInformations[] alternatives;
        
        public GroupMonsterStaticInformationsWithAlternatives()
        {
        }
        
        public GroupMonsterStaticInformationsWithAlternatives(Types.MonsterInGroupLightInformations mainCreatureLightInfos, Types.MonsterInGroupInformations[] underlings, Types.AlternativeMonstersInGroupLightInformations[] alternatives)
         : base(mainCreatureLightInfos, underlings)
        {
            this.alternatives = alternatives;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)alternatives.Length);
            foreach (var entry in alternatives)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            alternatives = new Types.AlternativeMonstersInGroupLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 alternatives[i] = new Types.AlternativeMonstersInGroupLightInformations();
                 alternatives[i].Deserialize(reader);
            }
        }
        
    }
    
}