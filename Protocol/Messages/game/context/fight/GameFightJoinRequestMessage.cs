

// Generated on 12/11/2012 19:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightJoinRequestMessage : NetworkMessage
    {
        public const uint Id = 701;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fighterId;
        public int fightId;
        
        public GameFightJoinRequestMessage()
        {
        }
        
        public GameFightJoinRequestMessage(int fighterId, int fightId)
        {
            this.fighterId = fighterId;
            this.fightId = fightId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fighterId);
            writer.WriteInt(fightId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fighterId = reader.ReadInt();
            fightId = reader.ReadInt();
        }
        
    }
    
}