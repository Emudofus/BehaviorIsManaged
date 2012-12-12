

// Generated on 12/11/2012 19:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameRolePlayPlayerFightFriendlyRequestedMessage : NetworkMessage
    {
        public const uint Id = 5937;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public int sourceId;
        public int targetId;
        
        public GameRolePlayPlayerFightFriendlyRequestedMessage()
        {
        }
        
        public GameRolePlayPlayerFightFriendlyRequestedMessage(int fightId, int sourceId, int targetId)
        {
            this.fightId = fightId;
            this.sourceId = sourceId;
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteInt(sourceId);
            writer.WriteInt(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            sourceId = reader.ReadInt();
            if (sourceId < 0)
                throw new Exception("Forbidden value on sourceId = " + sourceId + ", it doesn't respect the following condition : sourceId < 0");
            targetId = reader.ReadInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
        }
        
    }
    
}