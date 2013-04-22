

// Generated on 04/17/2013 22:29:42
using System;
using System.Collections.Generic;
using System.Linq;
using BiM.Protocol.Types;
using BiM.Core.IO;
using BiM.Core.Network;

namespace BiM.Protocol.Messages
{
    public class GameFightLeaveMessage : NetworkMessage
    {
        public const uint Id = 721;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int charId;
        
        public GameFightLeaveMessage()
        {
        }
        
        public GameFightLeaveMessage(int charId)
        {
            this.charId = charId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(charId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            charId = reader.ReadInt();
        }
        
    }
    
}