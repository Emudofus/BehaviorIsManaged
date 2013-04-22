

// Generated on 04/17/2013 22:30:07
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameFightFighterNamedInformations : GameFightFighterInformations
    {
        public const short Id = 158;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string name;
        public Types.PlayerStatus status;
        
        public GameFightFighterNamedInformations()
        {
        }
        
        public GameFightFighterNamedInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, sbyte teamId, bool alive, Types.GameFightMinimalStats stats, string name, Types.PlayerStatus status)
         : base(contextualId, look, disposition, teamId, alive, stats)
        {
            this.name = name;
            this.status = status;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
            status.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
            status = new Types.PlayerStatus();
            status.Deserialize(reader);
        }
        
    }
    
}