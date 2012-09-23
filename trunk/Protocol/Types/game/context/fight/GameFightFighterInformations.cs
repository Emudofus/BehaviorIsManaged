

// Generated on 09/23/2012 22:27:09
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Core.IO;

namespace BiM.Protocol.Types
{
    public class GameFightFighterInformations : GameContextActorInformations
    {
        public const short Id = 143;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte teamId;
        public bool alive;
        public Types.GameFightMinimalStats stats;
        
        public GameFightFighterInformations()
        {
        }
        
        public GameFightFighterInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, sbyte teamId, bool alive, Types.GameFightMinimalStats stats)
         : base(contextualId, look, disposition)
        {
            this.teamId = teamId;
            this.alive = alive;
            this.stats = stats;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(teamId);
            writer.WriteBoolean(alive);
            writer.WriteShort(stats.TypeId);
            stats.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            teamId = reader.ReadSByte();
            if (teamId < 0)
                throw new Exception("Forbidden value on teamId = " + teamId + ", it doesn't respect the following condition : teamId < 0");
            alive = reader.ReadBoolean();
            stats = Types.ProtocolTypeManager.GetInstance<Types.GameFightMinimalStats>(reader.ReadShort());
            stats.Deserialize(reader);
        }
        
    }
    
}