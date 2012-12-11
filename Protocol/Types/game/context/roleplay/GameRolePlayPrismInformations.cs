

// Generated on 12/11/2012 19:44:33
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameRolePlayPrismInformations : GameRolePlayActorInformations
    {
        public const short Id = 161;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.ActorAlignmentInformations alignInfos;
        
        public GameRolePlayPrismInformations()
        {
        }
        
        public GameRolePlayPrismInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, Types.ActorAlignmentInformations alignInfos)
         : base(contextualId, look, disposition)
        {
            this.alignInfos = alignInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            alignInfos.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            alignInfos = new Types.ActorAlignmentInformations();
            alignInfos.Deserialize(reader);
        }
        
    }
    
}