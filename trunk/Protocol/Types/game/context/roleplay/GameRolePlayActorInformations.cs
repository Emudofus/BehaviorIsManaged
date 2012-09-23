

// Generated on 09/23/2012 22:27:09
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameRolePlayActorInformations : GameContextActorInformations
    {
        public const short Id = 141;
        public override short TypeId
        {
            get { return Id; }
        }
        
        
        public GameRolePlayActorInformations()
        {
        }
        
        public GameRolePlayActorInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition)
         : base(contextualId, look, disposition)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}