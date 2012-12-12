

// Generated on 12/11/2012 19:44:23
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ChallengeFightJoinRefusedMessage : NetworkMessage
    {
        public const uint Id = 5908;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int playerId;
        public sbyte reason;
        
        public ChallengeFightJoinRefusedMessage()
        {
        }
        
        public ChallengeFightJoinRefusedMessage(int playerId, sbyte reason)
        {
            this.playerId = playerId;
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(playerId);
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            reason = reader.ReadSByte();
        }
        
    }
    
}