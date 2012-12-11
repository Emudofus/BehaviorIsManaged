

// Generated on 12/11/2012 19:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameRolePlayAggressionMessage : NetworkMessage
    {
        public const uint Id = 6073;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int attackerId;
        public int defenderId;
        
        public GameRolePlayAggressionMessage()
        {
        }
        
        public GameRolePlayAggressionMessage(int attackerId, int defenderId)
        {
            this.attackerId = attackerId;
            this.defenderId = defenderId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(attackerId);
            writer.WriteInt(defenderId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            attackerId = reader.ReadInt();
            if (attackerId < 0)
                throw new Exception("Forbidden value on attackerId = " + attackerId + ", it doesn't respect the following condition : attackerId < 0");
            defenderId = reader.ReadInt();
            if (defenderId < 0)
                throw new Exception("Forbidden value on defenderId = " + defenderId + ", it doesn't respect the following condition : defenderId < 0");
        }
        
    }
    
}