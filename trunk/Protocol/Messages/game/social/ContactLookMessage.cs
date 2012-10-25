

// Generated on 10/25/2012 10:42:54
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class ContactLookMessage : NetworkMessage
    {
        public const uint Id = 5934;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int requestId;
        public string playerName;
        public int playerId;
        public Types.EntityLook look;
        
        public ContactLookMessage()
        {
        }
        
        public ContactLookMessage(int requestId, string playerName, int playerId, Types.EntityLook look)
        {
            this.requestId = requestId;
            this.playerName = playerName;
            this.playerId = playerId;
            this.look = look;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(requestId);
            writer.WriteUTF(playerName);
            writer.WriteInt(playerId);
            look.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            playerName = reader.ReadUTF();
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            look = new Types.EntityLook();
            look.Deserialize(reader);
        }
        
    }
    
}