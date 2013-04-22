

// Generated on 04/17/2013 22:29:46
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameRolePlayArenaFighterStatusMessage : NetworkMessage
    {
        public const uint Id = 6281;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public int playerId;
        public bool accepted;
        
        public GameRolePlayArenaFighterStatusMessage()
        {
        }
        
        public GameRolePlayArenaFighterStatusMessage(int fightId, int playerId, bool accepted)
        {
            this.fightId = fightId;
            this.playerId = playerId;
            this.accepted = accepted;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteInt(playerId);
            writer.WriteBoolean(accepted);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            accepted = reader.ReadBoolean();
        }
        
    }
    
}