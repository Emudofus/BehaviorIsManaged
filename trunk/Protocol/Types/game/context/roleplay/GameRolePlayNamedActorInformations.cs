

// Generated on 09/23/2012 22:27:09
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameRolePlayNamedActorInformations : GameRolePlayActorInformations
    {
        public const short Id = 154;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string name;
        
        public GameRolePlayNamedActorInformations()
        {
        }
        
        public GameRolePlayNamedActorInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name)
         : base(contextualId, look, disposition)
        {
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
        }
        
    }
    
}