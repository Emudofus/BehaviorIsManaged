

// Generated on 04/17/2013 22:29:45
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameRolePlayPlayerFightFriendlyAnswerMessage : NetworkMessage
    {
        public const uint Id = 5732;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public bool accept;
        
        public GameRolePlayPlayerFightFriendlyAnswerMessage()
        {
        }
        
        public GameRolePlayPlayerFightFriendlyAnswerMessage(int fightId, bool accept)
        {
            this.fightId = fightId;
            this.accept = accept;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteBoolean(accept);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            accept = reader.ReadBoolean();
        }
        
    }
    
}